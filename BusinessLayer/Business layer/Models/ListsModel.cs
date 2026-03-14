namespace WebApi.BusinessLayer.Models;

public class ListsModel
{
    public ListsModel(int id, string listName, int createdByUser, DateTime createdDate)
    {
        this.Id = id;
        this.ListName = listName;
        this.CreatedByUser = createdByUser;
        this.CreatedDate = createdDate;
    }

    public int Id { get; set; }

    public string ListName { get; set; }

    public int CreatedByUser { get; set; }

    public DateTime CreatedDate { get; set; }

    public IList<DataLayer.DataLayer.Entities.Task>? Tasks { get; } = new List<DataLayer.DataLayer.Entities.Task>();
}
