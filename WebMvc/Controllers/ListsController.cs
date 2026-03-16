using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebMvc.Api;
using WebMvc.Api.Dtos;
using WebMvc.Models.ViewModels;
using WebMvc.Services;

namespace WebMvc.Controllers;

[Authorize]
public sealed class ListsController : Controller
{
    private readonly ITodoApiClient api;
    private readonly CurrentUserService currentUser;
    private readonly TodoApiOptions apiOptions;
    private readonly ILogger<ListsController> logger;

    public ListsController(
        ITodoApiClient api,
        CurrentUserService currentUser,
        IOptions<TodoApiOptions> apiOptions,
        ILogger<ListsController> logger)
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
            var lists = await this.api.GetListsAsync();
            var sharedLinks = await this.api.GetSharedListsAsync();

            var owned = lists
                .Where(l => l.CreatedByUser == apiUserId.Value)
                .OrderByDescending(l => l.CreatedDate)
                .ToArray();

            var sharedListIds = sharedLinks
                .Where(s => s.AssignedUserId == apiUserId.Value)
                .Select(s => s.ToDoListId)
                .Distinct()
                .ToHashSet();

            var sharedLists = lists
                .Where(l => sharedListIds.Contains(l.Id) && l.CreatedByUser != apiUserId.Value)
                .OrderBy(l => l.ListName)
                .ToArray();

            this.ViewData["SharedLists"] = sharedLists;
            return this.View(Pagination.Page(owned, page, pageSize));
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Index");
            return this.ApiError("Failed to load lists", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Index");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Index");
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
            var (accessibleListIds, lists) = await this.GetAccessibleListIdsAndListsAsync(apiUserId.Value);
            if (!accessibleListIds.Contains(id))
            {
                return this.Forbid();
            }

            var list = lists.FirstOrDefault(l => l.Id == id) ?? await this.api.GetListByIdAsync(id);
            if (list is null)
            {
                return this.NotFound();
            }

            var tasks = await this.api.GetTasksAsync();
            var inList = tasks
                .Where(t => t.ListId == id)
                .OrderBy(t => t.TaskFinishDate)
                .ThenBy(t => t.TaskName)
                .ToArray();

            return this.View(new ListDetailsViewModel
            {
                List = list,
                Tasks = Pagination.Page(inList, page, pageSize),
                AccessibleListIds = accessibleListIds.OrderBy(x => x).ToArray(),
            });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Details");
            return this.ApiError("Failed to load list details", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Details");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Details");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpGet]
    public IActionResult Create()
        => this.View(new ListEditViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ListEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var lists = await this.api.GetListsAsync();
            var newId = IdAllocator.NextId(lists.Select(l => l.Id));

            var dto = new ListDto
            {
                Id = newId,
                ListName = model.ListName.Trim(),
                CreatedByUser = apiUserId.Value,
                CreatedDate = DateTime.UtcNow,
            };

            await this.api.AddListAsync(dto);
            return this.RedirectToAction(nameof(this.Details), new { id = newId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Create");
            return this.ApiError("Failed to create list", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Create");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Create");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
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
            var list = await this.api.GetListByIdAsync(id);
            if (list is null)
            {
                return this.NotFound();
            }

            if (list.CreatedByUser != apiUserId.Value)
            {
                return this.Forbid();
            }

            return this.View(new ListEditViewModel { ListName = list.ListName });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Edit(GET)");
            return this.ApiError("Failed to load list", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Edit(GET)");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Edit(GET)");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ListEditViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var list = await this.api.GetListByIdAsync(id);
            if (list is null)
            {
                return this.NotFound();
            }

            if (list.CreatedByUser != apiUserId.Value)
            {
                return this.Forbid();
            }

            list.ListName = model.ListName.Trim();
            await this.api.UpdateListAsync(list);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Edit(POST)");
            return this.ApiError("Failed to update list", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Edit(POST)");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Edit(POST)");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var list = await this.api.GetListByIdAsync(id);
            if (list is null)
            {
                return this.NotFound();
            }

            if (list.CreatedByUser != apiUserId.Value)
            {
                return this.Forbid();
            }

            var shared = await this.api.GetSharedListsAsync();
            foreach (var s in shared.Where(x => x.ToDoListId == id))
            {
                await this.api.DeleteSharedListAsync(s.Id);
            }

            await this.api.DeleteListAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Delete");
            return this.ApiError("Failed to delete list", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Delete");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Delete");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Share(int id)
    {
        var apiUserId = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var list = await this.api.GetListByIdAsync(id);
            if (list is null)
            {
                return this.NotFound();
            }

            if (list.CreatedByUser != apiUserId.Value)
            {
                return this.Forbid();
            }

            var users = await this.api.GetUsersAsync();
            this.ViewData["Users"] = users.Where(u => u.Id != apiUserId.Value).OrderBy(u => u.Username).ToArray();

            return this.View(new ShareListViewModel { ToDoListId = id });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Share(GET)");
            return this.ApiError("Failed to load users", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Share(GET)");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Share(GET)");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Share(ShareListViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            try
            {
                var apiUserId = await this.currentUser.GetApiUserIdAsync();
                var users = await this.api.GetUsersAsync();
                if (apiUserId is int id)
                {
                    this.ViewData["Users"] = users.Where(u => u.Id != id).OrderBy(u => u.Username).ToArray();
                }
                else
                {
                    this.ViewData["Users"] = users.OrderBy(u => u.Username).ToArray();
                }
            }
            catch (TodoApiException)
            {
            }
            catch (HttpRequestException)
            {
            }
            catch (TaskCanceledException)
            {
            }
            return this.View(model);
        }

        var apiUserId2 = await this.currentUser.GetApiUserIdAsync();
        if (apiUserId2 is null)
        {
            return this.ApiUnavailable("Cannot resolve your API user id.");
        }

        try
        {
            var list = await this.api.GetListByIdAsync(model.ToDoListId);
            if (list is null)
            {
                return this.NotFound();
            }

            if (list.CreatedByUser != apiUserId2.Value)
            {
                return this.Forbid();
            }

            var shared = await this.api.GetSharedListsAsync();
            var alreadyShared = shared.Any(s => s.ToDoListId == model.ToDoListId && s.AssignedUserId == model.AssignedUserId);
            if (!alreadyShared)
            {
                var newId = IdAllocator.NextId(shared.Select(s => s.Id));
                await this.api.AddSharedListAsync(new SharedListDto
                {
                    Id = newId,
                    ToDoListId = model.ToDoListId,
                    UserWhoAssignsIs = apiUserId2.Value,
                    AssignedUserId = model.AssignedUserId,
                });
            }

            return this.RedirectToAction(nameof(this.Details), new { id = model.ToDoListId });
        }
        catch (TodoApiException ex)
        {
            this.logger.LogWarning(ex, "Todo API error in Lists/Share(POST)");
            return this.ApiError("Failed to share list", ex);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning(ex, "HTTP error in Lists/Share(POST)");
            return this.ApiUnavailable("Cannot reach the WebApi.", ex);
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogWarning(ex, "Todo API request timed out in Lists/Share(POST)");
            return this.ApiUnavailable("The WebApi request timed out.", ex);
        }
    }

    private async Task<(HashSet<int> AccessibleListIds, IReadOnlyList<ListDto> Lists)> GetAccessibleListIdsAndListsAsync(int apiUserId)
    {
        var lists = await this.api.GetListsAsync();
        var sharedLinks = await this.api.GetSharedListsAsync();

        var accessible = lists
            .Where(l => l.CreatedByUser == apiUserId)
            .Select(l => l.Id)
            .ToHashSet();

        foreach (var id in sharedLinks.Where(s => s.AssignedUserId == apiUserId).Select(s => s.ToDoListId))
        {
            _ = accessible.Add(id);
        }

        return (accessible, lists);
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


