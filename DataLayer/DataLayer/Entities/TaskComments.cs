using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DataLayer.Entities;

public class TaskComments : BaseEntity
{
    [ForeignKey("Task")]
    public int TaskId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public string CommentText { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public User? User { get; set; }

    public Task? Task { get; set; }
}
