using MediatR;
using Microsoft.Extensions.Logging;
using static Pokedex.Framework.Logging.LoggingHelpers;

namespace Pokedex.Application.Abstractions.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
  private readonly ILogger _logger;

  public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(next);

    try
    {
      _logger.LogInformation("Handling request of type {RequestType}", typeof(TRequest).Name);

      var response = await next().ConfigureAwait(false);

      _logger.LogInformation("Successfully handled request of type {RequestType}", typeof(TRequest).Name);

      return response;
    }
    catch (Exception exception) when (False(() => LogRequestProcessingError(exception)))
    {
      throw;
    }

    void LogRequestProcessingError(Exception exception)
    {
      _logger.LogError(
        exception,
        "An error occurred while processing request of type {RequestType}: {Error}",
        typeof(TRequest).Name,
        exception.Message);
    }
  }
}
