namespace WebApi.BusinessLayer.Models;

public class TagsModel
{
    public TagsModel(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public int Id { get; set; }

    public string Name { get; set; }
}
