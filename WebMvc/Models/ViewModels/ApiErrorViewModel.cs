namespace WebMvc.Models.ViewModels;

public sealed class ApiErrorViewModel
{
    public required string Title { get; init; }

    public string? Message { get; init; }

    public int? StatusCode { get; init; }

    public string? Details { get; init; }
}
