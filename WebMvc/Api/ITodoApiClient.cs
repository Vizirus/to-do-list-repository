using WebMvc.Api.Dtos;

namespace WebMvc.Api;

public interface ITodoApiClient
{
    Task<IReadOnlyList<ListDto>> GetListsAsync(CancellationToken cancellationToken = default);
    Task<ListDto?> GetListByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddListAsync(ListDto dto, CancellationToken cancellationToken = default);
    Task UpdateListAsync(ListDto dto, CancellationToken cancellationToken = default);
    Task DeleteListAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SharedListDto>> GetSharedListsAsync(CancellationToken cancellationToken = default);
    Task AddSharedListAsync(SharedListDto dto, CancellationToken cancellationToken = default);
    Task DeleteSharedListAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TaskDto>> GetTasksAsync(CancellationToken cancellationToken = default);
    Task<TaskDto?> GetTaskByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddTaskAsync(TaskDto dto, CancellationToken cancellationToken = default);
    Task UpdateTaskAsync(TaskDto dto, CancellationToken cancellationToken = default);
    Task DeleteTaskAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TaskCommentDto>> GetCommentsAsync(CancellationToken cancellationToken = default);
    Task AddCommentAsync(TaskCommentDto dto, CancellationToken cancellationToken = default);
    Task UpdateCommentAsync(TaskCommentDto dto, CancellationToken cancellationToken = default);
    Task DeleteCommentAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TaskStatusDto>> GetTaskStatusesAsync(CancellationToken cancellationToken = default);
    Task AddTaskStatusAsync(TaskStatusDto dto, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TagDto>> GetTagsAsync(CancellationToken cancellationToken = default);
    Task AddTagAsync(TagDto dto, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TaskTagDto>> GetTaskTagsAsync(CancellationToken cancellationToken = default);
    Task AddTaskTagAsync(TaskTagDto dto, CancellationToken cancellationToken = default);
    Task DeleteTaskTagAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task AddUserAsync(UserDto dto, CancellationToken cancellationToken = default);
}

