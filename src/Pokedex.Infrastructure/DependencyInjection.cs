using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pokedex.Domain.Pokemons;
using Pokedex.Infrastructure.PokeApis;
using Pokedex.Infrastructure.PokeApis.Configuration;
using Polly;

namespace Pokedex.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
  {
    ArgumentNullException.ThrowIfNull(services);
    ArgumentNullException.ThrowIfNull(configuration);

    services.AddOptions<PokeApiOptions>()
      .Bind(configuration.GetRequiredSection(PokeApiOptions.PokeApi))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    services.AddHttpClient<IPokeApiHttpClient, PokeApiHttpClient>((serviceProvider, httpClient) =>
    {
      var pokeApiOptions = serviceProvider
        .GetRequiredService<IOptions<PokeApiOptions>>()
        .Value;

      httpClient.BaseAddress = pokeApiOptions.BaseAddress;
    })
    .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: _ => TimeSpan.FromMilliseconds(600)
        )
    );

    services.AddTransient<IPokemonRepository, PokemonHttpRepository>();

    return services;
  }
}
