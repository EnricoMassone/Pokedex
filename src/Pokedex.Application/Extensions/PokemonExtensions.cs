using Pokedex.Application.Pokemons.GetPokemon;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Extensions;

public static class PokemonExtensions
{
  public static GetPokemonQueryResponse ToGetPokemonQueryResponse(
    this Pokemon pokemon)
  {
    ArgumentNullException.ThrowIfNull(pokemon);

    return new GetPokemonQueryResponse
    {
      Description = pokemon.Description,
      Habitat = pokemon.Habitat,
      IsLegendary = pokemon.IsLegendary,
      Name = pokemon.Name
    };
  }
}
