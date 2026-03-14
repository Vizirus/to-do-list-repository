namespace WebApi.BusinessLayer.Models;

public class TaskCommentsModel
{
    public TaskCommentsModel(int id, int taskId, int userId, string commentText, DateTime createdDate)
    {
        this.Id = id;
        this.TaskId = taskId;
        this.UserId = userId;
        this.CommentText = commentText;
        this.CreatedDate = createdDate;
    }

    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; }

    public DateTime CreatedDate { get; set; }
}
