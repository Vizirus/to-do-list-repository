using WebMvc.Api.Dtos;
using WebMvc.Services;

namespace WebMvc.Models.ViewModels;

public sealed class AssignedTasksViewModel
{
    public required PagedResult<TaskDto> Tasks { get; init; }
}
