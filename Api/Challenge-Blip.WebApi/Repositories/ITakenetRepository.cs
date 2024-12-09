using Challenge_Blip.WebApi.Models;

namespace Challenge_Blip.WebApi.Repositories;

public interface ITakenetRepository
{
    Task<List<Repository>> GetRepositoriesAsync();
}
