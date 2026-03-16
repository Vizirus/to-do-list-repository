namespace WebMvc.Api.Dtos;

public sealed class SharedListDto
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    public int UserWhoAssignsIs { get; set; }

    public int AssignedUserId { get; set; }
}

