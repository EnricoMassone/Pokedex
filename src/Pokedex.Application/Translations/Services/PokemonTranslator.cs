using Pokedex.Application.Translations.Abstractions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;
using Pokedex.Framework.Patterns.Strategies;

namespace Pokedex.Application.Translations.Services;

public sealed class PokemonTranslator : IPokemonTranslator
{
  private readonly IStrategySelector<Pokemon, TranslatedDescription> _strategySelector;

  public PokemonTranslator(IStrategySelector<Pokemon, TranslatedDescription> strategySelector)
  {
    _strategySelector = strategySelector ?? throw new ArgumentNullException(nameof(strategySelector));
  }

  public async Task<Pokemon> TranslateAsync(Pokemon pokemon, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(pokemon);

    var translationStrategy = _strategySelector.GetRequiredMatchingStrategy(pokemon);

    var translatedDescription = await translationStrategy.HandleAsync(
      pokemon,
      cancellationToken).ConfigureAwait(false);

    return pokemon with { Description = translatedDescription };
  }
}
