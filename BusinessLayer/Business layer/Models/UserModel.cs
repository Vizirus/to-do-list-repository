namespace WebApi.BusinessLayer.Models;

public class UserModel
{
    public UserModel(int id, string username, string email, string passwordHash, DateTime createdDate)
    {
        this.Id = id;
        this.Username = username;
        this.Email = email;
        this.PasswordHash = passwordHash;
        this.CreatedDate = createdDate;
    }

    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public DateTime CreatedDate { get; set; }
}
