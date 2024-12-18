﻿namespace Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;

public sealed record FunTranslationsApiResponse
{
  public required ContentsDto Contents { get; init; }
}

public sealed record ContentsDto
{
  public required string Translated { get; init; }
}
