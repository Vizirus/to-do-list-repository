using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models.ViewModels;

public sealed class CommentCreateViewModel
{
    [Required]
    public int TaskId { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 1)]
    [Display(Name = "Comment")]
    public string CommentText { get; set; } = string.Empty;
}
