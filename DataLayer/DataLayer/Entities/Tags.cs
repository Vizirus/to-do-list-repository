namespace DataLayer.DataLayer.Entities;

public class Tags : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    //Binding properties. Igonre when writing tests
    public ICollection<TaskTags>? TaskTags { get; set; }
}
