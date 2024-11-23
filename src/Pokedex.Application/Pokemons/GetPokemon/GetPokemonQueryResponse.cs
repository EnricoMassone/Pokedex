namespace Pokedex.Application.Pokemons.GetPokemon;

public sealed record GetPokemonQueryResponse
{
  public required string Name { get; init; }
  public required string? Description { get; init; }
  public required string? Habitat { get; init; }
  public required bool IsLegendary { get; init; }
}
