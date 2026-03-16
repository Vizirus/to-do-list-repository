using WebMvc.Api.Dtos;
using WebMvc.Services;

namespace WebMvc.Models.ViewModels;

public sealed class SearchResultsViewModel
{
    public required string Query { get; init; }

    public required PagedResult<TaskDto> Tasks { get; init; }
}
