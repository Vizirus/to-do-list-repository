namespace DataLayer.DataLayer.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}
