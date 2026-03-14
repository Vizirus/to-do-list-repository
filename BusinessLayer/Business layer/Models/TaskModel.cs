using DataLayer.DataLayer.Entities;

namespace WebApi.BusinessLayer.Models;

public class TaskModel
{
    public TaskModel(int id, int listId, string taskName, string taskDescription, DateTime taskStartDate, DateTime taskFinishDate, int statusId, int assigndUserId, int taskStatusIds)
    {
        this.Id = id;
        this.ListId = listId;
        this.TaskName = taskName;
        this.TaskDescription = taskDescription;
        this.TaskStartDate = taskStartDate;
        this.TaskFinishDate = taskFinishDate;
        this.StatusId = statusId;
        this.AssigndUserId = assigndUserId;
        this.TaskStatusIds = taskStatusIds;
    }

    public int Id { get; set; }

    public int ListId { get; set; }

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }

    public DateTime TaskStartDate { get; set; }

    public DateTime TaskFinishDate { get; set; }

    public int StatusId { get; set; }

    public int AssigndUserId { get; set; }

    public int TaskStatusIds { get; set; }

    public IList<TaskComments> TaskComments { get; } = new List<TaskComments>();

    public IList<TaskTags> TaskTags { get; } = new List<TaskTags>();
}
