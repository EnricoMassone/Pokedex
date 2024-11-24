using Pokedex.Domain.Abstractions;

namespace Pokedex.Infrastructure.FunTranslationsApis;

public sealed class FunTranslationsApiHttpClient : IFunTranslationsApiHttpClient
{
  public async Task<Result<FunTranslationsApiResponse>> ApplyYodaTranslationAsync(
    Text text,
    CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }

  public async Task<Result<FunTranslationsApiResponse>> ApplyShakespeareTranslationAsync(
    Text text,
    CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
