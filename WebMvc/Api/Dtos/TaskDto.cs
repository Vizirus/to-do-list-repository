namespace WebMvc.Api.Dtos;

public sealed class TaskDto
{
    public int Id { get; set; }

    public int ListId { get; set; }

    public string TaskName { get; set; } = string.Empty;

    public string TaskDescription { get; set; } = string.Empty;

    public DateTime TaskStartDate { get; set; }

    public DateTime TaskFinishDate { get; set; }

    public int StatusId { get; set; }

    public int AssigndUserId { get; set; }
}

