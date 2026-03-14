using DataLayer.DataLayer.Entities;

namespace WebApi.BusinessLayer.Models;

public class SharedListsModel
{
    public SharedListsModel(int id, int toDoListId, int userWhoAssignsIs, int assignedUserId)
    {
        this.Id = id;
        this.ToDoListId = toDoListId;
        this.UserWhoAssignsIs = userWhoAssignsIs;
        this.AssignedUserId = assignedUserId;
    }

    public int Id { get; set; }

    public int ToDoListId { get; set; }

    public int UserWhoAssignsIs { get; set; }

    public int AssignedUserId { get; set; }

    public IList<User> Users { get; } = new List<User>();
}
