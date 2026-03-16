using System.Net;

namespace WebMvc.Api;

public sealed class TodoApiException : Exception
{
    public TodoApiException()
    {
    }

    public TodoApiException(string message)
        : base(message)
    {
    }

    public TodoApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public TodoApiException(HttpStatusCode statusCode, string? responseBody, string? message = null, Exception? innerException = null)
        : base(message ?? $"Todo API request failed with {(int)statusCode} ({statusCode}).", innerException)
    {
        this.StatusCode = statusCode;
        this.ResponseBody = responseBody;
    }

    public HttpStatusCode StatusCode { get; }

    public string? ResponseBody { get; }
}
