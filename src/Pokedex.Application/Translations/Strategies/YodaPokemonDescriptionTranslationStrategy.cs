using Microsoft.Extensions.Logging;
using Pokedex.Application.Abstractions;
using Pokedex.Application.Translations.Abstractions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;
using Pokedex.Framework.Patterns.Strategies;

namespace Pokedex.Application.Translations.Strategies;

public sealed class YodaPokemonDescriptionTranslationStrategy : Strategy<Pokemon, TranslatedDescription>
{
  private readonly IFunTranslationsApiHttpClient _httpClient;
  private readonly ILogger _logger;

  public YodaPokemonDescriptionTranslationStrategy(
    IFunTranslationsApiHttpClient httpClient,
    ILogger<YodaPokemonDescriptionTranslationStrategy> logger)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public override bool CanHandle(Pokemon data)
  {
    ArgumentNullException.ThrowIfNull(data);
    return data.Habitat == PokemonHabitats.Cave || data.IsLegendary;
  }

  protected override async Task<TranslatedDescription> ComputeResultAsync(Pokemon data, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(data);

    if (string.IsNullOrWhiteSpace(data.Description))
    {
      return new TranslatedDescription(data.Description);
    }

    var translationResult = await _httpClient.ApplyYodaTranslationAsync(
      new Text(data.Description),
      cancellationToken).ConfigureAwait(false);

    if (translationResult.IsFailure)
    {
      return HandleFailedTranslation(translationResult.Error);
    }

    var translatedDescription = translationResult.Value.Contents.Translated;

    return new TranslatedDescription(translatedDescription);

    TranslatedDescription HandleFailedTranslation(Error error)
    {
      _logger.LogWarning(
        "Yoda translation of Pokemon description failed with error code {ErrorCode}. Reason: {Reason}",
        error.Code,
        error.Description);

      return new TranslatedDescription(data.Description);
    }
  }
}
