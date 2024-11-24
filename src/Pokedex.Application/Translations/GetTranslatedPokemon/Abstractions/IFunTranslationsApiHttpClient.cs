using Pokedex.Domain.Abstractions;

namespace Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;

public interface IFunTranslationsApiHttpClient
{
  Task<Result<FunTranslationsApiResponse>> ApplyShakespeareTranslationAsync(
    Text text,
    CancellationToken cancellationToken);

  Task<Result<FunTranslationsApiResponse>> ApplyYodaTranslationAsync(
    Text text,
    CancellationToken cancellationToken);
}