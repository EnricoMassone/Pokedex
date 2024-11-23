using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pokedex.Domain.Pokemons;
using Pokedex.Infrastructure.PokemonApis;
using Pokedex.Infrastructure.PokemonApis.Configuration;

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
    });

    services.AddTransient<IPokemonRepository, PokemonHttpRepository>();

    return services;
  }
}
