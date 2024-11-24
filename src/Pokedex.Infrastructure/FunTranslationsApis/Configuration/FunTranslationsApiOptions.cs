using System.ComponentModel.DataAnnotations;

namespace Pokedex.Infrastructure.FunTranslationsApis.Configuration;

public sealed class FunTranslationsApiOptions
{
  public const string FunTranslationsApi = "FunTranslationsApi";

  [Required(ErrorMessage = "FunTranslationsApi:BaseAddress is mandatory configuration")]
  public Uri BaseAddress { get; init; } = default!;
}
