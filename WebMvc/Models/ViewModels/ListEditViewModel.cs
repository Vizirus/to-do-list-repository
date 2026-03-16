using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models.ViewModels;

public sealed class ListEditViewModel
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    [Display(Name = "List name")]
    public string ListName { get; set; } = string.Empty;
}
