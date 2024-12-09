using Challenge_Blip.WebApi.Models;
using Challenge_Blip.WebApi.Repositories;
using MediatR;

namespace Challenge_Blip.WebApi.Features;

public class GetRepositoriesHandler(ITakenetRepository repository) : IRequestHandler<GetRepositoriesQuery, List<Repository>>
{
    private readonly ITakenetRepository _repository = repository;

    public async Task<List<Repository>> Handle(GetRepositoriesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetRepositoriesAsync();
    }
}
