namespace Pokedex.Framework.Patterns.Strategies;

public interface IStrategySelector<TData, TResult>
{
  IStrategy<TData, TResult> GetRequiredMatchingStrategy(TData data);
}