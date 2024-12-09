using MediatR;
using Challenge_Blip.WebApi.Models;

namespace Challenge_Blip.WebApi.Features;

public record GetRepositoriesQuery : IRequest<List<Repository>>;