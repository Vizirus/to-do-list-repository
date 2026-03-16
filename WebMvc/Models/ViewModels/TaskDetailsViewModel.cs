using Microsoft.AspNetCore.Mvc.Rendering;
using WebMvc.Api.Dtos;

namespace WebMvc.Models.ViewModels;

public sealed class TaskDetailsViewModel
{
    public required TaskDto Task { get; init; }

    public required string ListName { get; init; }

    public required IReadOnlyList<TaskCommentDto> Comments { get; init; }

    public required IReadOnlyList<(TaskTagDto Link, TagDto Tag)> Tags { get; init; }

    public required SelectList Statuses { get; init; }

    public required SelectList Users { get; init; }

    public required SelectList AllTags { get; init; }

    public CommentCreateViewModel NewComment { get; init; } = new();
}
