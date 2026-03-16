using WebMvc.Api.Dtos;

namespace WebMvc.Models.ViewModels;

public sealed class TagsIndexViewModel
{
    public required IReadOnlyList<TagDto> Tags { get; init; }

    public TagCreateViewModel NewTag { get; init; } = new();
}
