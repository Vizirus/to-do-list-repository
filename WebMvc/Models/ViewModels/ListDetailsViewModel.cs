using WebMvc.Api.Dtos;
using WebMvc.Services;

namespace WebMvc.Models.ViewModels;

public sealed class ListDetailsViewModel
{
    public required ListDto List { get; init; }

    public required PagedResult<TaskDto> Tasks { get; init; }

    public required IReadOnlyList<int> AccessibleListIds { get; init; }
}
