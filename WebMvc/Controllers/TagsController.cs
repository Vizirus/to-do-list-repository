using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebMvc.Api;
using WebMvc.Api.Dtos;
using WebMvc.Models.ViewModels;
using WebMvc.Services;

namespace WebMvc.Controllers;

[Authorize]
public sealed class TagsController : Controller
{
    private readonly ITodoApiClient api;
    private readonly CurrentUserService currentUser;
    private readonly TodoApiOptions apiOptions;
    private readonly ILogger<TagsController> logger;

    public TagsController(
        ITodoApiClient api,
        CurrentUserService currentUser,
        IOptions<TodoApiOptions> apiOptions,
        ILogger<TagsController> logger)
    {
        this.api = api;
        this.currentUser = currentUser;
        this.apiOptions = apiOptions.Value;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var tags = await this.api.GetTagsAsync();
            return this.View(new TagsIndexViewModel { Tags = tags.OrderBy(t => t.Name).ToArray() });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tags/Index");
            return this.ApiError("Failed to load tags", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tags/Index");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tags/Index");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagCreateViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            try
            {
                var tags = await this.api.GetTagsAsync();
                return this.View(nameof(this.Index), new TagsIndexViewModel { Tags = tags.OrderBy(t => t.Name).ToArray(), NewTag = model });
            }
            catch (TodoApiException)
            {
                return this.View(nameof(this.Index), new TagsIndexViewModel { Tags = Array.Empty<TagDto>(), NewTag = model });
            }
            catch (HttpRequestException)
            {
                return this.View(nameof(this.Index), new TagsIndexViewModel { Tags = Array.Empty<TagDto>(), NewTag = model });
            }
            catch (TaskCanceledException)
            {
                return this.View(nameof(this.Index), new TagsIndexViewModel { Tags = Array.Empty<TagDto>(), NewTag = model });
            }
        }

        try
        {
            var tags = await this.api.GetTagsAsync();
            var existing = tags.FirstOrDefault(t => string.Equals(t.Name, model.Name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existing is not null)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = existing.Id });
            }

            var newId = IdAllocator.NextId(tags.Select(t => t.Id));
            await this.api.AddTagAsync(new TagDto { Id = newId, Name = model.Name.Trim() });
            return this.RedirectToAction(nameof(this.Details), new { id = newId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tags/Create");
            return this.ApiError("Failed to create tag", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tags/Create");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tags/Create");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 10)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var tags = await this.api.GetTagsAsync();
            var tag = tags.FirstOrDefault(t => t.Id == id);
            if (tag is null)
            {
                return this.NotFound();
            }

            var lists = await this.api.GetListsAsync();
            var shared = await this.api.GetSharedListsAsync();
            var accessibleListIds = lists
                .Where(l => l.CreatedByUser == apiUserId.Value)
                .Select(l => l.Id)
                .ToHashSet();
            foreach (var lid in shared.Where(s => s.AssignedUserId == apiUserId.Value).Select(s => s.ToDoListId))
            {
                _ = accessibleListIds.Add(lid);
            }

            var taskTags = await this.api.GetTaskTagsAsync();
            var taskIds = taskTags.Where(tt => tt.TagId == id).Select(tt => tt.TaskId).ToHashSet();

            var tasks = await this.api.GetTasksAsync();
            var matched = tasks
                .Where(t => taskIds.Contains(t.Id))
                .Where(t => accessibleListIds.Contains(t.ListId))
                .OrderBy(t => t.TaskFinishDate)
                .ThenBy(t => t.TaskName)
                .ToArray();

            return this.View(new TagDetailsViewModel
            {
                Tag = tag,
                Tasks = Pagination.Page(matched, page, pageSize),
            });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tags/Details");
            return this.ApiError("Failed to load tag", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tags/Details");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tags/Details");
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



