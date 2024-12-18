﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;
using Pokedex.Domain.Pokemons;
using Pokedex.Infrastructure.FunTranslationsApis;
using Pokedex.Infrastructure.FunTranslationsApis.Configuration;
using Pokedex.Infrastructure.PokeApis;
using Pokedex.Infrastructure.PokeApis.Configuration;
using Polly;

namespace Pokedex.Infrastructure;

public static class DependencyInjectionConfiguration
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

    services.AddOptions<FunTranslationsApiOptions>()
      .Bind(configuration.GetRequiredSection(FunTranslationsApiOptions.FunTranslationsApi))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    services.AddHttpClient<IFunTranslationsApiHttpClient, FunTranslationsApiHttpClient>((serviceProvider, httpClient) =>
    {
      var funTranslationsApiOptions = serviceProvider
        .GetRequiredService<IOptions<FunTranslationsApiOptions>>()
        .Value;

      httpClient.BaseAddress = funTranslationsApiOptions.BaseAddress;
    })
    .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: _ => TimeSpan.FromMilliseconds(600)
        )
    );

    return services;
  }
}
