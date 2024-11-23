using Pokedex.Domain.Abstractions;

namespace Pokedex.Infrastructure.PokemonApis;

public interface IPokeApiHttpClient
{
  Task<Option<PokemonApiResponse>> GetPokemonByNameAsync(string name, CancellationToken cancellationToken);
}