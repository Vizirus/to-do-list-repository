using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DataLayer.Entities;

public abstract class BaseEntity
{
    [Key]
    [Column]
    public int Id { get; set; }
}
