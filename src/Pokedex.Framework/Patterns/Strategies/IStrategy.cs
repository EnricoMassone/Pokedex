namespace Pokedex.Framework.Patterns.Strategies;

public interface IStrategy<TData, TResult>
{
  bool CanHandle(TData data);
  Task<TResult> HandleAsync(TData data, CancellationToken cancellationToken);
}
