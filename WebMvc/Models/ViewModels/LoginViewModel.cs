using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models.ViewModels;

public sealed class LoginViewModel
{
    [Required]
    [Display(Name = "Username or email")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}
