using DataLayer.DataLayer.Entities;

namespace BusinessLayer.BusinessLayer.Models;

public class TaskModel
{
    public TaskModel(int id, int listId, string taskName, string taskDescription, DateTime taskStartDate, DateTime taskFinishDate, int statusId, int assigndUserId)
        : this(id, listId, taskName, taskDescription, taskStartDate, taskFinishDate, statusId)
    {
        this.AssigndUserId = assigndUserId;
    }

    public TaskModel(int id, int listId, string taskName, string taskDescription, DateTime taskStartDate, DateTime taskFinishDate, int statusId)
    {
        this.Id = id;
        this.ListId = listId;
        this.TaskName = taskName;
        this.TaskDescription = taskDescription;
        this.TaskStartDate = taskStartDate;
        this.TaskFinishDate = taskFinishDate;
        this.StatusId = statusId;
    }

    public int Id { get; set; }

    public int ListId { get; set; }

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }

    public DateTime TaskStartDate { get; set; }

    public DateTime TaskFinishDate { get; set; }

    public int StatusId { get; set; }

    public int AssigndUserId { get; set; }

    public IList<TaskComments> TaskComments { get; } = new List<TaskComments>();

    public IList<TaskTags> TaskTags { get; } = new List<TaskTags>();
}
