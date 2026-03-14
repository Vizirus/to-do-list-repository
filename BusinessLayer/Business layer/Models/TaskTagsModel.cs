namespace WebApi.BusinessLayer.Models;

public class TaskTagsModel
{
    public TaskTagsModel(int id, int taskId, int tagId)
    {
        this.Id = id;
        this.TaskId = taskId;
        this.TagId = tagId;
    }

    public int Id { get; set; }

    public int TaskId { get; set; }

    public int TagId { get; set; }
}
