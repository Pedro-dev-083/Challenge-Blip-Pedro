using Challenge_Blip.WebApi.Exceptions;
using Challenge_Blip.WebApi.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Challenge_Blip.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TakenetController(IMediator mediator, ILogger<TakenetController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<TakenetController> _logger = logger;

    [HttpGet("repositories")]
    public async Task<IActionResult> GetRepositories()
    {
        try
        {
            var query = new GetRepositoriesQuery();
            var result = await _mediator.Send(query);

            if (result == null || result.Count == 0)
            {
                _logger.LogWarning("No repositories found for the user.");
                return NotFound("No repositories found for the user.");
            }

            return Ok(result);
        }
        catch (GitHubApiException ex)
        {
            _logger.LogError(ex, "GitHub API error occurred while fetching repositories.");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "GitHub API is unavailable. Please try again later.");
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Request to GitHub API timed out.");
            return StatusCode(StatusCodes.Status504GatewayTimeout, "Request timed out. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching repositories.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please contact support.");
        }
    }
}
