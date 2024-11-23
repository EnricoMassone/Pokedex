using System.Text.Json.Serialization;

namespace Pokedex.Infrastructure.PokemonApis;

public sealed record PokemonSpeciesApiResponse
{
  public required string Name { get; init; }

  [JsonPropertyName("is_legendary")]
  public required bool IsLegendary { get; init; }

  public HabitatDto? Habitat { get; init; }

  [JsonPropertyName("flavor_text_entries")]
  public IReadOnlyList<FlavorTextDto> FlavorTextEntries { get; init; } = [];
}

public sealed record HabitatDto
{
  public required string Name { get; init; }
}

public sealed record LanguageDto
{
  public required string Name { get; init; }
}

public sealed record FlavorTextDto
{
  [JsonPropertyName("flavor_text")]
  public required string FlavorText { get; init; }
  public required LanguageDto Language { get; init; }
}
