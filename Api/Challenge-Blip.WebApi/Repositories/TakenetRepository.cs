using Challenge_Blip.WebApi.Models;
using System.Text.Json;

namespace Challenge_Blip.WebApi.Repositories;

public class TakenetRepository(HttpClient httpClient, ILogger<TakenetRepository> logger) : ITakenetRepository
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<TakenetRepository> _logger = logger;
    private const string Username = "takenet";

    public async Task<List<Repository>> GetRepositoriesAsync()
    {
        var url = $"https://api.github.com/users/{Username}/repos?per_page=5&sort=created";
        _logger.LogInformation("Fetching repositories for user: {Username}", Username);

        try
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("C# App");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github.v3+json");

            var repositories = await _httpClient.GetFromJsonAsync<List<Repository>>(url);

            _logger.LogInformation("Successfully fetched {Count} repositories", repositories?.Count ?? 0);

            return repositories ?? [];
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP error occurred while fetching repositories for user: {Username}", Username);
            throw new HttpRequestException("A network error occurred while accessing the GitHub API. Please try again later.", httpEx);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Error parsing the response for user: {Username}", Username);
            throw new JsonException("An error occurred while parsing the GitHub API response. Please contact support.", jsonEx);
        }
        catch (OperationCanceledException cancelEx)
        {
            _logger.LogWarning(cancelEx, "Operation was canceled while fetching repositories for user: {Username}", Username);
            throw new OperationCanceledException("The operation timed out. Please try again later.", cancelEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching repositories for user: {Username}", Username);
            throw new InvalidOperationException("An unexpected error occurred. Please try again later.", ex);
        }
    }
}
