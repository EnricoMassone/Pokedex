using Pokedex.Domain.Pokemons;

namespace Pokedex.Infrastructure.PokemonApis;

public sealed class PokeApiClient
{
  private readonly HttpClient _httpClient;

  public PokeApiClient(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
  }

  public async Task<Pokemon> GetPokemonByNameAsync(string name, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
