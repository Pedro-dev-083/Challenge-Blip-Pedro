namespace Challenge_Blip.WebApi.Exceptions;

public class GitHubApiException(string message, int? statusCode = null, Exception? innerException = null) : Exception(message, innerException)
{
    public int? StatusCode { get; } = statusCode;
}
