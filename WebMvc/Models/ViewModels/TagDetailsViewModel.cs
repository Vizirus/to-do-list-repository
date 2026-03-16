using WebMvc.Api.Dtos;
using WebMvc.Services;

namespace WebMvc.Models.ViewModels;

public sealed class TagDetailsViewModel
{
    public required TagDto Tag { get; init; }

    public required PagedResult<TaskDto> Tasks { get; init; }
}
