namespace WebApi.BusinessLayer.Models;

public class TaskStatusesModel
{
    public TaskStatusesModel(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public IList<DataLayer.DataLayer.Entities.Task> Tasks { get; } = new List<DataLayer.DataLayer.Entities.Task>();
}
