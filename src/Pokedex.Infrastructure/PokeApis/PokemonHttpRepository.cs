using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Infrastructure.PokeApis;

public sealed class PokemonHttpRepository : IPokemonRepository
{
  private readonly IPokeApiHttpClient _pokeApiHttpClient;

  public PokemonHttpRepository(IPokeApiHttpClient pokeApiHttpClient)
  {
    _pokeApiHttpClient = pokeApiHttpClient ?? throw new ArgumentNullException(nameof(pokeApiHttpClient));
  }

  public async Task<Option<Pokemon>> GetByNameAsync(Name name, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(name);

    var pokeApiResponse = await _pokeApiHttpClient.GetPokemonByNameAsync(
      name,
      cancellationToken).ConfigureAwait(false);

    if (!pokeApiResponse.HasValue)
    {
      return Option<Pokemon>.None;
    }

    var habitat = pokeApiResponse.Value.Habitat?.Name;

    var description = pokeApiResponse
      .Value
      .FlavorTextEntries
      .FirstOrDefault(entry => entry.Language.Name == CultureCodes.English)?.FlavorText;

    return new Pokemon
    {
      Description = description,
      Habitat = habitat,
      Name = new Name(pokeApiResponse.Value.Name),
      IsLegendary = pokeApiResponse.Value.IsLegendary
    };
  }
}
