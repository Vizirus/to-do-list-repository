using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models.ViewModels;

public sealed class TagCreateViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
}
