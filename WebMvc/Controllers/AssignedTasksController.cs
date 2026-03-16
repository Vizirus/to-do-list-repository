using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebMvc.Api;
using WebMvc.Models.ViewModels;
using WebMvc.Services;

namespace WebMvc.Controllers;

[Authorize]
public sealed class AssignedTasksController : Controller
{
    private readonly ITodoApiClient api;
    private readonly CurrentUserService currentUser;
    private readonly TodoApiOptions apiOptions;
    private readonly ILogger<AssignedTasksController> logger;

    public AssignedTasksController(
        ITodoApiClient api,
        CurrentUserService currentUser,
        IOptions<TodoApiOptions> apiOptions,
        ILogger<AssignedTasksController> logger)
    {
        this.api = api;
        this.currentUser = currentUser;
        this.apiOptions = apiOptions.Value;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var tasks = await this.api.GetTasksAsync();
            var assigned = tasks
                .Where(t => t.AssigndUserId == apiUserId.Value)
                .OrderBy(t => t.TaskFinishDate)
                .ThenBy(t => t.TaskName)
                .ToArray();

            return this.View(new AssignedTasksViewModel
            {
                Tasks = Pagination.Page(assigned, page, pageSize),
            });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in AssignedTasks/Index");
            return this.ApiError("Failed to load assigned tasks", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in AssignedTasks/Index");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in AssignedTasks/Index");
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
