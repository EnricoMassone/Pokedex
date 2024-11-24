using Microsoft.Extensions.Logging;
using Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;
using Pokedex.Framework.Patterns.Strategies;

namespace Pokedex.Application.Translations.GetTranslatedPokemon.Services;

public sealed class PokemonTranslator : IPokemonTranslator
{
  private readonly IStrategySelector<Pokemon, TranslatedDescription> _strategySelector;
  private readonly ILogger _logger;

  public PokemonTranslator(
    IStrategySelector<Pokemon, TranslatedDescription> strategySelector,
    ILogger<PokemonTranslator> logger)
  {
    _strategySelector = strategySelector ?? throw new ArgumentNullException(nameof(strategySelector));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<Pokemon> TranslateAsync(Pokemon pokemon, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(pokemon);

    var translationStrategy = _strategySelector.GetRequiredMatchingStrategy(pokemon);

    var translatedDescription = await translationStrategy.HandleAsync(
      pokemon,
      cancellationToken).ConfigureAwait(false);

    _logger.LogInformation(
      "Successfully translated Pokemon description using strategy {Strategy}",
      translationStrategy.GetType().Name
    );

    return pokemon with { Description = translatedDescription };
  }
}
