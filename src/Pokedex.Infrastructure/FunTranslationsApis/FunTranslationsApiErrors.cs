using Pokedex.Domain.Abstractions;

namespace Pokedex.Infrastructure.FunTranslationsApis;

public static class FunTranslationsApiErrors
{
  public static readonly Error TooManyRequests = new(
        "FunTranslationsApi.TooManyRequests",
        "FunTranslationsApi rate limit threshold has been reached");
}
