namespace WebMvc.Api;

public sealed class TodoApiOptions
{
    public const string SectionName = "TodoApi";

    public string BaseUrl { get; set; } = "http://localhost:5128/";

    public string? BearerToken { get; set; }
}


