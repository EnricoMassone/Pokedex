namespace Pokedex.Framework.Patterns.Strategies;

public abstract class Strategy<TData, TResult> : IStrategy<TData, TResult>
{
  protected string Name => this.GetType().Name;

  public abstract bool CanHandle(TData data);

  public async Task<TResult> HandleAsync(TData data, CancellationToken cancellationToken)
  {
    if (this.CanHandle(data))
    {
      throw new InvalidOperationException(
        $"Strategy {this.Name} cannot handle the provided data of type {typeof(TData).Name}"
      );
    }

    return await ComputeResultAsync(data, cancellationToken).ConfigureAwait(false);
  }

  protected abstract Task<TResult> ComputeResultAsync(TData data, CancellationToken cancellationToken);
}
