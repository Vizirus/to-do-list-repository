using Microsoft.AspNetCore.Identity;

namespace WebMvc.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public int ApiUserId { get; set; }
}

