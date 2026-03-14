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
    public DateTime TaskStartDate { get; set; }

    [Column]
    public DateTime TaskFinishDate { get; set; }

    [Column]
    [ForeignKey("TaskStatusIds")]
    public int StatusId { get; set; }

    [Column]
    [ForeignKey("UserId")]
    public int AssigndUserId { get; set; }

    //Binding properties. Igonre when writing tests
    public TaskStatuses? TaskStatusIds { get; set; }

    public User? UserId { get; set; }

    public Lists? ToDoListId { get; set; }

    public IList<TaskComments>? TaskComments { get; set; }

    public IList<TaskTags>? TaskTags { get; set; }
}
