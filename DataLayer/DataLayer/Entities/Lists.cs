using System.ComponentModel.DataAnnotations.Schema;
namespace DataLayer.DataLayer.Entities;

public class Lists : BaseEntity
{
    public string ListName { get; set; } = string.Empty;

    [ForeignKey("User")]
    public int CreatedByUser { get; set; }

    public DateTime CreatedDate { get; set; }

    public User? User { get; set; }
}
