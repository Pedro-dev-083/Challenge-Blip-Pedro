using Challenge_Blip.WebApi.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

namespace Challenge_Blip.WebApi.Tests.Repositories;
public class TakenetRepositoryTests
{
    private readonly Mock<HttpMessageHandler> _httpHandlerMock;
    private readonly TakenetRepository _repository;

    public TakenetRepositoryTests()
    {
        _httpHandlerMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_httpHandlerMock.Object);
        var logger = new Mock<ILogger<TakenetRepository>>();

        _repository = new TakenetRepository(httpClient, logger.Object);
    }

    [Fact]
    public async Task GetRepositoriesAsync_ShouldReturnRepositories()
    {
        // Arrange
        var jsonResponse = "[{\"Name\":\"Repo1\"}]";
        _httpHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _repository.GetRepositoriesAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Repo1");
    }
}
