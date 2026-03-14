using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DataLayer.Entities;

public class TaskTags : BaseEntity
{
    [ForeignKey("Task")]
    public int TaskId { get; set; }

    [ForeignKey("Tag")]
    public int TagId { get; set; }

    public Tags? Tag { get; set; }

    public Task? Task { get; set; }
}
