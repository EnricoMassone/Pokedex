using Pokedex.Application.Translations.Abstractions;
using Pokedex.Domain.Abstractions;
using System.Net.Http.Json;

namespace Pokedex.Infrastructure.FunTranslationsApis;

public sealed class FunTranslationsApiHttpClient : IFunTranslationsApiHttpClient
{
  private static readonly Uri ShakespeareEndpointUri = new("translate/shakespeare", UriKind.Relative);
  private static readonly Uri YodaEndpointUri = new("translate/yoda", UriKind.Relative);

  private readonly HttpClient _httpClient;

  public FunTranslationsApiHttpClient(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
  }

  public Task<Result<FunTranslationsApiResponse>> ApplyYodaTranslationAsync(
    Text text,
    CancellationToken cancellationToken) =>
    this.TranslateAsync(
        text,
        YodaEndpointUri,
        cancellationToken);

  public Task<Result<FunTranslationsApiResponse>> ApplyShakespeareTranslationAsync(
    Text text,
    CancellationToken cancellationToken) =>
      this.TranslateAsync(
        text,
        ShakespeareEndpointUri,
        cancellationToken);

  private async Task<Result<FunTranslationsApiResponse>> TranslateAsync(
    Text text,
    Uri translationEndpointUri,
    CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(text);

    var request = new FunTranslationsApiRequest
    {
      Text = text,
    };
    var httpResponseMessage = await _httpClient.PostAsJsonAsync(
      translationEndpointUri,
      request,
      cancellationToken).ConfigureAwait(false);

    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
    {
      return Result.Failure<FunTranslationsApiResponse>(FunTranslationsApiErrors.TooManyRequests);
    }

    httpResponseMessage.EnsureSuccessStatusCode();

    var response = await httpResponseMessage
      .Content
      .ReadFromJsonAsync<FunTranslationsApiResponse>(cancellationToken).ConfigureAwait(false);

    if (response is null)
    {
      return Result.Failure<FunTranslationsApiResponse>(FunTranslationsApiErrors.NullResponseJsonContent);
    }

    return Result.Success(response);
  }
}
