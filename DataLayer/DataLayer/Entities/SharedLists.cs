using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DataLayer.Entities;

public class SharedLists : BaseEntity
{
    [ForeignKey("Lists")]
    public int ToDoListId { get; set; }

    [ForeignKey("User")]
    public int UserWhoAssignsIs { get; set; }

    [ForeignKey("User2")]
    public int AssignedUserId { get; set; }

    public User? User { get; set; }

    public User? User2 { get; set; }

    public Lists? Lists { get; set; }
}
