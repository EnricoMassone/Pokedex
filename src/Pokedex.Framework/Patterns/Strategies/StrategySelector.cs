using Pokedex.Framework.Patterns.Strategies.Exceptions;
using System.Collections.Immutable;

namespace Pokedex.Framework.Patterns.Strategies;

public sealed class StrategySelector<TData, TResult> : IStrategySelector<TData, TResult>
{
  private readonly ImmutableArray<IStrategy<TData, TResult>> _strategies;

  public StrategySelector(IEnumerable<IStrategy<TData, TResult>> strategies)
  {
    ArgumentNullException.ThrowIfNull(strategies);

    _strategies = strategies.ToImmutableArray();
  }

  public IStrategy<TData, TResult> GetRequiredMatchingStrategy(TData data)
  {
    var matchingStrategy = _strategies.FirstOrDefault(
      strategy => strategy.CanHandle(data)
    );

    if (matchingStrategy is null)
    {
      throw new MissingMatchingStrategyException(
        $"There is no matching strategy for the provided data of type {typeof(TData).Name}"
      );
    }

    return matchingStrategy;
  }
}
