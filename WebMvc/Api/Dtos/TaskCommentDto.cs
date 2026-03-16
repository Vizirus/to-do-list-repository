namespace WebMvc.Api.Dtos;

public sealed class TaskCommentDto
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}

