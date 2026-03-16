using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models.ViewModels;

public sealed class SearchViewModel
{
    [Display(Name = "Search")]
    [StringLength(200)]
    public string? Query { get; set; }
}
