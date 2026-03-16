using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models.ViewModels;

public sealed class ShareListViewModel
{
    [Required]
    public int ToDoListId { get; set; }

    [Required]
    [Display(Name = "Share with user")]
    public int AssignedUserId { get; set; }
}
