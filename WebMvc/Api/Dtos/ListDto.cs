namespace WebMvc.Api.Dtos;

public sealed class ListDto
{
    public int Id { get; set; }

    public string ListName { get; set; } = string.Empty;

    public int CreatedByUser { get; set; }

    public DateTime CreatedDate { get; set; }
}

