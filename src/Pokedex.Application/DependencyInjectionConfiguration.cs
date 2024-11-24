using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Abstractions.Behaviors;

namespace Pokedex.Application;

public static class DependencyInjectionConfiguration
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    ArgumentNullException.ThrowIfNull(services);

    services.AddMediatR(configuration =>
    {
      configuration.RegisterServicesFromAssembly(typeof(DependencyInjectionConfiguration).Assembly);
      configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

    return services;
  }
}
