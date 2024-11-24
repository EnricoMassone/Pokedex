using Pokedex.Application.Pokemons.GetPokemon;
using Pokedex.Application.Translations.GetTranslatedPokemon;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Extensions;

internal static class PokemonExtensions
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

  public static GetTranslatedPokemonQueryResponse ToGetTranslatedPokemonQueryResponse(
    this Pokemon pokemon)
  {
    ArgumentNullException.ThrowIfNull(pokemon);

    return new GetTranslatedPokemonQueryResponse
    {
      Description = pokemon.Description,
      Habitat = pokemon.Habitat,
      IsLegendary = pokemon.IsLegendary,
      Name = pokemon.Name
    };
  }
}
