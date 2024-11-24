namespace Pokedex.Application.Abstractions;

public sealed record TranslatedDescription(string? Value)
{
  public static implicit operator string?(TranslatedDescription translatedDescription) => translatedDescription.Value;
}
