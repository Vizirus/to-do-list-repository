using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DataLayer.Entities;

public class Task : BaseEntity
{
    [Column]
    [ForeignKey("ToDoListId")]
    public int ListId { get; set; }

    [Column]
    public string TaskName { get; set; } = string.Empty;

    [Column]
    public string TaskDescription { get; set; } = string.Empty;

    [Column]
    public DateOnly TaskStartDate { get; set; }

    [Column]
    public DateOnly TaskFinishDate { get; set; }

    [Column]
    [ForeignKey("TaskStatusIds")]
    public int StatusId { get; set; }

    [Column]
    [ForeignKey("UserId")]
    public int AssigndUserId { get; set; }

    public int TaskStatusIds { get; set; }

    public User? UserId { get; set; }

    public Lists? ToDoListId { get; set; }
}
