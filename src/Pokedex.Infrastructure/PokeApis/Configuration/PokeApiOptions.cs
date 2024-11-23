using System.ComponentModel.DataAnnotations;

namespace Pokedex.Infrastructure.PokeApis.Configuration;

public sealed class PokeApiOptions
{
  public const string PokeApi = "PokeApi";

  [Required(ErrorMessage = "PokeApi:BaseAddress is mandatory configuration")]
  public Uri BaseAddress { get; init; } = default!;
}
