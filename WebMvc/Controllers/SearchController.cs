using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebMvc.Api;
using WebMvc.Models.ViewModels;
using WebMvc.Services;

namespace WebMvc.Controllers;

[Authorize]
public sealed class SearchController : Controller
{
    private readonly ITodoApiClient api;
    private readonly CurrentUserService currentUser;
    private readonly TodoApiOptions apiOptions;
    private readonly ILogger<SearchController> logger;

    public SearchController(
        ITodoApiClient api,
        CurrentUserService currentUser,
        IOptions<TodoApiOptions> apiOptions,
        ILogger<SearchController> logger)
    {
        this.api = api;
        this.currentUser = currentUser;
        this.apiOptions = apiOptions.Value;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Index(string? q = null)
        => this.View(new SearchViewModel { Query = q });

    [HttpGet]
    public async Task<IActionResult> Results(string q, int page = 1, int pageSize = 10)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        if (string.IsNullOrWhiteSpace(q))
        {
            return this.RedirectToAction(nameof(this.Index));
        }

        try
        {
            var lists = await this.api.GetListsAsync();
            var shared = await this.api.GetSharedListsAsync();

            var accessibleListIds = lists
                .Where(l => l.CreatedByUser == apiUserId.Value)
                .Select(l => l.Id)
                .ToHashSet();

            foreach (var id in shared.Where(s => s.AssignedUserId == apiUserId.Value).Select(s => s.ToDoListId))
            {
                _ = accessibleListIds.Add(id);
            }

            var tasks = await this.api.GetTasksAsync();
            var term = q.Trim();

            var matches = tasks
                .Where(t => accessibleListIds.Contains(t.ListId))
                .Where(t =>
                    (!string.IsNullOrEmpty(t.TaskName) && t.TaskName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(t.TaskDescription) && t.TaskDescription.Contains(term, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(t => t.TaskFinishDate)
                .ThenBy(t => t.TaskName)
                .ToArray();

            return this.View(new SearchResultsViewModel
            {
                Query = term,
                Tasks = Pagination.Page(matches, page, pageSize),
            });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Search/Results");
            return this.ApiError("Search failed", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Search/Results");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Search/Results");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    private IActionResult ApiUnavailable(string message, Exception? ex = null)
        => this.View("~/Views/Shared/ApiError.cshtml", new ApiErrorViewModel
        {
            Title = "API is not available",
            Message = $"{message} Make sure the WebApi is running and WebMvc TodoApi:BaseUrl points to it (current: {this.apiOptions.BaseUrl}).",
            Details = ex?.Message,
        });

    private IActionResult ApiError(string title, TodoApiException ex)
        => this.View("~/Views/Shared/ApiError.cshtml", new ApiErrorViewModel
        {
            Title = title,
            Message = $"The API returned an error (current: {this.apiOptions.BaseUrl}).",
            StatusCode = (int)ex.StatusCode,
            Details = ex.ResponseBody,
        });
}
