namespace Pokedex.Infrastructure.FunTranslationsApis;

public sealed record FunTranslationsApiRequest
{
  public required string Text { get; init; }
}
