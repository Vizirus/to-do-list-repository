namespace DataLayer.DataLayer.Entities;

public class TaskStatuses : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    //Binding properties. Igonre when writing tests
    public IList<Task>? Tasks { get; set; }
}
