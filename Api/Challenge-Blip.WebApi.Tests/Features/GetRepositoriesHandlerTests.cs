using Challenge_Blip.WebApi.Features;
using Challenge_Blip.WebApi.Models;
using Challenge_Blip.WebApi.Repositories;
using FluentAssertions;
using Moq;

namespace Challenge_Blip.WebApi.Tests.Features;
public class GetRepositoriesHandlerTests
{
    private readonly Mock<ITakenetRepository> _repositoryMock;
    private readonly GetRepositoriesHandler _handler;

    public GetRepositoriesHandlerTests()
    {
        _repositoryMock = new Mock<ITakenetRepository>();
        _handler = new GetRepositoriesHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnRepositories()
    {
        var repositories = new List<Repository> { new() { Name = "Repo1" } };
        _repositoryMock.Setup(r => r.GetRepositoriesAsync()).ReturnsAsync(repositories);

        var query = new GetRepositoriesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(repositories);
    }
}
