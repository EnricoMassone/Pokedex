using Pokedex.Application.Abstractions;
using Pokedex.Application.Translations.Abstractions;
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
    throw new NotImplementedException();
  }
}
