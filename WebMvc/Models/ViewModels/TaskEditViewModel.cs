using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models.ViewModels;

public sealed class TaskEditViewModel : IValidatableObject
{
    [Required]
    public int ListId { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 1)]
    [Display(Name = "Task name")]
    public string TaskName { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    [Display(Name = "Description")]
    public string TaskDescription { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Start date")]
    public DateTime TaskStartDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Finish date")]
    public DateTime TaskFinishDate { get; set; } = DateTime.Today;

    [Required]
    [Display(Name = "Status")]
    public int StatusId { get; set; }

    [Required]
    [Display(Name = "Assignee")]
    public int AssigndUserId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (this.TaskFinishDate.Date < this.TaskStartDate.Date)
        {
            yield return new ValidationResult("Finish date must be on or after start date.", new[] { nameof(this.TaskFinishDate) });
        }
    }
}
