using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using WebMvc.Api;
using WebMvc.Api.Dtos;
using WebMvc.Models.ViewModels;
using WebMvc.Services;

namespace WebMvc.Controllers;

[Authorize]
public sealed class TasksController : Controller
{
    private readonly ITodoApiClient api;
    private readonly CurrentUserService currentUser;
    private readonly TodoApiOptions apiOptions;
    private readonly ILogger<TasksController> logger;

    public TasksController(
        ITodoApiClient api,
        CurrentUserService currentUser,
        IOptions<TodoApiOptions> apiOptions,
        ILogger<TasksController> logger)
    {
        this.api = api;
        this.currentUser = currentUser;
        this.apiOptions = apiOptions.Value;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int listId)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id. ");
        }

        try
        {
            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(listId))
            {
                return this.Forbid();
            }

            var statuses = await this.api.GetTaskStatusesAsync();
            var users = await this.api.GetUsersAsync();

            this.ViewData["Statuses"] = new SelectList(statuses.OrderBy(s => s.Id), "Id", "Name", statuses.FirstOrDefault()?.Id);
            this.ViewData["Users"] = new SelectList(users.OrderBy(u => u.Username), "Id", "Username", apiUserId.Value);

            return this.View(new TaskEditViewModel
            {
                ListId = listId,
                TaskStartDate = DateTime.Today,
                TaskFinishDate = DateTime.Today,
                StatusId = statuses.FirstOrDefault()?.Id ?? 1,
                AssigndUserId = apiUserId.Value,
            });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/Create(GET)");
            return this.ApiError("Failed to load task form", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/Create(GET)");
            return this.ApiUnavailable("Cannot reach the WebApi. ", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/Create(GET)");
            return this.ApiUnavailable("The WebApi request timed out. ", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskEditViewModel model)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        if (!this.ModelState.IsValid)
        {
            await this.PopulateTaskDropdownsAsync(apiUserId.Value, model.StatusId, model.AssigndUserId);
            return this.View(model);
        }

        try
        {
            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(model.ListId))
            {
                return this.Forbid();
            }

            var tasks = await this.api.GetTasksAsync();
            var newId = IdAllocator.NextId(tasks.Select(t => t.Id));

            var dto = new TaskDto
            {
                Id = newId,
                ListId = model.ListId,
                TaskName = model.TaskName.Trim(),
                TaskDescription = model.TaskDescription.Trim(),
                TaskStartDate = model.TaskStartDate,
                TaskFinishDate = model.TaskFinishDate,
                StatusId = model.StatusId,
                AssigndUserId = model.AssigndUserId,
            };

            await this.api.AddTaskAsync(dto);
            return this.RedirectToAction(nameof(this.Details), new { id = newId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/Create(POST)");
            return this.ApiError("Failed to create task", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/Create(POST)");
            return this.ApiUnavailable("Cannot reach the WebApi. ", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/Create(POST)");
            return this.ApiUnavailable("The WebApi request timed out. ", ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var task = await this.api.GetTaskByIdAsync(id);
            if (task is null)
            {
                return this.NotFound();
            }

            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(task.ListId))
            {
                return this.Forbid();
            }

            await this.PopulateTaskDropdownsAsync(apiUserId.Value, task.StatusId, task.AssigndUserId);
            this.ViewData["TaskId"] = id;

            return this.View(new TaskEditViewModel
            {
                ListId = task.ListId,
                TaskName = task.TaskName,
                TaskDescription = task.TaskDescription,
                TaskStartDate = task.TaskStartDate.ToLocalTime().Date,
                TaskFinishDate = task.TaskFinishDate.ToLocalTime().Date,
                StatusId = task.StatusId,
                AssigndUserId = task.AssigndUserId,
            });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/Edit(GET)");
            return this.ApiError("Failed to load task", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/Edit(GET)");
            return this.ApiUnavailable("Cannot reach the WebApi. ", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/Edit(GET)");
            return this.ApiUnavailable("The WebApi request timed out. ", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskEditViewModel model)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        if (!this.ModelState.IsValid)
        {
            await this.PopulateTaskDropdownsAsync(apiUserId.Value, model.StatusId, model.AssigndUserId);
            this.ViewData["TaskId"] = id;
            return this.View(model);
        }

        try
        {
            var task = await this.api.GetTaskByIdAsync(id);
            if (task is null)
            {
                return this.NotFound();
            }

            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(task.ListId))
            {
                return this.Forbid();
            }

            task.TaskName = model.TaskName.Trim();
            task.TaskDescription = model.TaskDescription.Trim();
            task.TaskStartDate = model.TaskStartDate;
            task.TaskFinishDate = model.TaskFinishDate;
            task.StatusId = model.StatusId;
            task.AssigndUserId = model.AssigndUserId;

            await this.api.UpdateTaskAsync(task);
            return this.RedirectToAction(nameof(this.Details), new { id });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/Edit(POST)");
            return this.ApiError("Failed to update task", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/Edit(POST)");
            return this.ApiUnavailable("Cannot reach the WebApi ", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/Edit(POST)");
            return this.ApiUnavailable("The WebApi request timed out ", ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var task = await this.api.GetTaskByIdAsync(id);
            if (task is null)
            {
                return this.NotFound();
            }

            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(task.ListId))
            {
                return this.Forbid();
            }

            var lists = await this.api.GetListsAsync();
            var listName = lists.FirstOrDefault(l => l.Id == task.ListId)?.ListName ?? $"List #{task.ListId}";

            var comments = (await this.api.GetCommentsAsync())
                .Where(c => c.TaskId == id)
                .OrderByDescending(c => c.CreatedDate)
                .ToArray();

            var taskTags = (await this.api.GetTaskTagsAsync())
                .Where(tt => tt.TaskId == id)
                .ToArray();

            var tags = await this.api.GetTagsAsync();
            var tagMap = tags.ToDictionary(t => t.Id);

            var joinedTags = taskTags
                .Select(link => (Link: link, Tag: tagMap.TryGetValue(link.TagId, out var t) ? t : new TagDto { Id = link.TagId, Name = $"Tag #{link.TagId}" }))
                .OrderBy(x => x.Tag.Name)
                .ToArray();

            var statuses = await this.api.GetTaskStatusesAsync();
            var users = await this.api.GetUsersAsync();

            return this.View(new TaskDetailsViewModel
            {
                Task = task,
                ListName = listName,
                Comments = comments,
                Tags = joinedTags,
                Statuses = new SelectList(statuses.OrderBy(s => s.Id), "Id", "Name", task.StatusId),
                Users = new SelectList(users.OrderBy(u => u.Username), "Id", "Username", task.AssigndUserId),
                AllTags = new SelectList(tags.OrderBy(t => t.Name), "Id", "Name"),
                NewComment = new CommentCreateViewModel { TaskId = id },
            });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/Details");
            return this.ApiError("Failed to load task details", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/Details");
            return this.ApiUnavailable("Cannot reach the WebApi ", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/Details");
            return this.ApiUnavailable("The WebApi request timed out ", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int listId)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(listId))
            {
                return this.Forbid();
            }

            var comments = await this.api.GetCommentsAsync();
            foreach (var c in comments.Where(c => c.TaskId == id))
            {
                await this.api.DeleteCommentAsync(c.Id);
            }

            var taskTags = await this.api.GetTaskTagsAsync();
            foreach (var tt in taskTags.Where(tt => tt.TaskId == id))
            {
                await this.api.DeleteTaskTagAsync(tt.Id);
            }

            await this.api.DeleteTaskAsync(id);
            return this.RedirectToAction(actionName: "Details", controllerName: "Lists", new { id = listId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/Delete");
            return this.ApiError("Failed to delete task", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/Delete");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/Delete");
            return this.ApiUnavailable("The WebApi request timed out ", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, int statusId)
        => await this.UpdateTaskAsync(id, task => task.StatusId = statusId);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reassign(int id, int assigndUserId)
        => await this.UpdateTaskAsync(id, task => task.AssigndUserId = assigndUserId);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(TaskDetailsViewModel model)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var comments = await this.api.GetCommentsAsync();
            var newId = IdAllocator.NextId(comments.Select(c => c.Id));

            await this.api.AddCommentAsync(new TaskCommentDto
            {
                Id = newId,
                TaskId = model.NewComment.TaskId,
                UserId = apiUserId.Value,
                CommentText = model.NewComment.CommentText.Trim(),
                CreatedDate = DateTime.UtcNow,
            });

            return this.RedirectToAction(nameof(this.Details), new { id = model.NewComment.TaskId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/AddComment");
            return this.ApiError("Failed to add comment", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/AddComment");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/AddComment");
            return this.ApiUnavailable("The WebApi request timed out!", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTag(int taskId, int tagId)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var task = await this.api.GetTaskByIdAsync(taskId);
            if (task is null)
            {
                return this.NotFound();
            }

            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(task.ListId))
            {
                return this.Forbid();
            }

            var taskTags = await this.api.GetTaskTagsAsync();
            if (!taskTags.Any(tt => tt.TaskId == taskId && tt.TagId == tagId))
            {
                var newId = IdAllocator.NextId(taskTags.Select(tt => tt.Id));
                await this.api.AddTaskTagAsync(new TaskTagDto { Id = newId, TaskId = taskId, TagId = tagId });
            }

            return this.RedirectToAction(nameof(this.Details), new { id = taskId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/AddTag");
            return this.ApiError("Failed to add tag", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/AddTag");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/AddTag");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveTag(int taskId, int taskTagId)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var task = await this.api.GetTaskByIdAsync(taskId);
            if (task is null)
            {
                return this.NotFound();
            }

            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(task.ListId))
            {
                return this.Forbid();
            }

            await this.api.DeleteTaskTagAsync(taskTagId);
            return this.RedirectToAction(nameof(this.Details), new { id = taskId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/RemoveTag");
            return this.ApiError("Failed to remove tag", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/RemoveTag");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/RemoveTag");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    private async Task<IActionResult> UpdateTaskAsync(int id, Action<TaskDto> mutate)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var task = await this.api.GetTaskByIdAsync(id);
            if (task is null)
            {
                return this.NotFound();
            }

            var accessibleListIds = await this.GetAccessibleListIdsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(task.ListId))
            {
                return this.Forbid();
            }

            mutate(task);
            await this.api.UpdateTaskAsync(task);
            return this.RedirectToAction(nameof(this.Details), new { id });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Tasks/UpdateTaskAsync");
            return this.ApiError("Failed to update task", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Tasks/UpdateTaskAsync");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Tasks/UpdateTaskAsync");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    private async Task PopulateTaskDropdownsAsync(int apiUserId, int? statusId, int? assigneeId)
    {
        var statuses = await this.api.GetTaskStatusesAsync();
        var users = await this.api.GetUsersAsync();

        this.ViewData["Statuses"] = new SelectList(statuses.OrderBy(s => s.Id), "Id", "Name", statusId ?? statuses.FirstOrDefault()?.Id);
        this.ViewData["Users"] = new SelectList(users.OrderBy(u => u.Username), "Id", "Username", assigneeId ?? apiUserId);
    }

    private async Task<HashSet<int>> GetAccessibleListIdsAsync(int apiUserId)
    {
        var lists = await this.api.GetListsAsync();
        var sharedLinks = await this.api.GetSharedListsAsync();

        var ids = lists.Where(l => l.CreatedByUser == apiUserId).Select(l => l.Id).ToHashSet();
        foreach (var id in sharedLinks.Where(s => s.AssignedUserId == apiUserId).Select(s => s.ToDoListId))
        {
            _ = ids.Add(id);
        }

        return ids;
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
