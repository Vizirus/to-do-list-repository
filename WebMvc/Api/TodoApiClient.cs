using System.Net;
using System.Text.Json;
using WebMvc.Api.Dtos;

namespace WebMvc.Api;

public sealed class TodoApiClient : ITodoApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly HttpClient httpClient;

    public TodoApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public Task<IReadOnlyList<ListDto>> GetListsAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<ListDto>("api/List", cancellationToken);

    public Task<ListDto?> GetListByIdAsync(int id, CancellationToken cancellationToken = default)
        => this.GetOrNullAsync<ListDto>($"api/List/{id}", cancellationToken);

    public Task AddListAsync(ListDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/List", dto, cancellationToken);

    public Task UpdateListAsync(ListDto dto, CancellationToken cancellationToken = default)
        => this.PutAsync("api/List", dto, cancellationToken);

    public Task DeleteListAsync(int id, CancellationToken cancellationToken = default)
        => this.DeleteAsync($"api/List/{id}", cancellationToken);

    public Task<IReadOnlyList<SharedListDto>> GetSharedListsAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<SharedListDto>("api/List/shared", cancellationToken);

    public Task AddSharedListAsync(SharedListDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/List/shared", dto, cancellationToken);

    public Task DeleteSharedListAsync(int id, CancellationToken cancellationToken = default)
        => this.DeleteAsync($"api/List/shared/{id}", cancellationToken);

    public Task<IReadOnlyList<TaskDto>> GetTasksAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<TaskDto>("api/Task", cancellationToken);

    public Task<TaskDto?> GetTaskByIdAsync(int id, CancellationToken cancellationToken = default)
        => this.GetOrNullAsync<TaskDto>($"api/Task/{id}", cancellationToken);

    public Task AddTaskAsync(TaskDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/Task", dto, cancellationToken);

    public Task UpdateTaskAsync(TaskDto dto, CancellationToken cancellationToken = default)
        => this.PutAsync("api/Task", dto, cancellationToken);

    public Task DeleteTaskAsync(int id, CancellationToken cancellationToken = default)
        => this.DeleteAsync($"api/Task/{id}", cancellationToken);

    public Task<IReadOnlyList<TaskCommentDto>> GetCommentsAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<TaskCommentDto>("api/TaskAdditional", cancellationToken);

    public Task AddCommentAsync(TaskCommentDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/TaskAdditional", dto, cancellationToken);

    public Task UpdateCommentAsync(TaskCommentDto dto, CancellationToken cancellationToken = default)
        => this.PutAsync("api/TaskAdditional", dto, cancellationToken);

    public Task DeleteCommentAsync(int id, CancellationToken cancellationToken = default)
        => this.DeleteAsync($"api/TaskAdditional/{id}", cancellationToken);

    public Task<IReadOnlyList<TaskStatusDto>> GetTaskStatusesAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<TaskStatusDto>("api/TaskAdditional/status", cancellationToken);

    public Task AddTaskStatusAsync(TaskStatusDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/TaskAdditional/status", dto, cancellationToken);

    public Task<IReadOnlyList<TagDto>> GetTagsAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<TagDto>("api/Tags", cancellationToken);

    public Task AddTagAsync(TagDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/Tags", dto, cancellationToken);

    public Task<IReadOnlyList<TaskTagDto>> GetTaskTagsAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<TaskTagDto>("api/Tags/tasktag", cancellationToken);

    public Task AddTaskTagAsync(TaskTagDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/Tags/tasktag", dto, cancellationToken);

    public Task DeleteTaskTagAsync(int id, CancellationToken cancellationToken = default)
        => this.DeleteAsync($"api/Tags/tasktag/{id}", cancellationToken);

    public Task<IReadOnlyList<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default)
        => this.GetListAsync<UserDto>("api/User", cancellationToken);

    public Task AddUserAsync(UserDto dto, CancellationToken cancellationToken = default)
        => this.PostAsync("api/User", dto, cancellationToken);

    private async Task<IReadOnlyList<T>> GetListAsync<T>(string url, CancellationToken cancellationToken)
    {
        var result = await this.GetOrNullAsync<List<T>>(url, cancellationToken);
        return (IReadOnlyList<T>?)result ?? Array.Empty<T>();
    }

    private async Task<T?> GetOrNullAsync<T>(string url, CancellationToken cancellationToken)
    {
        using var response = await this.httpClient.GetAsync(url, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await SafeReadBodyAsync(response, cancellationToken);
            throw new TodoApiException(response.StatusCode, body);
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
    }

    private async Task PostAsync<T>(string url, T payload, CancellationToken cancellationToken)
    {
        using var response = await this.httpClient.PostAsJsonAsync(url, payload, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private async Task PutAsync<T>(string url, T payload, CancellationToken cancellationToken)
    {
        using var response = await this.httpClient.PutAsJsonAsync(url, payload, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private async Task DeleteAsync(string url, CancellationToken cancellationToken)
    {
        using var response = await this.httpClient.DeleteAsync(url, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await SafeReadBodyAsync(response, cancellationToken);
        throw new TodoApiException(response.StatusCode, body);
    }

    private static async Task<string?> SafeReadBodyAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (ObjectDisposedException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (System.IO.IOException)
        {
            return null;
        }
    }
}
