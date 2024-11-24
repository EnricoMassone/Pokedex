using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Abstractions.Behaviors;
using Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;
using Pokedex.Application.Translations.GetTranslatedPokemon.Services;
using Pokedex.Application.Translations.GetTranslatedPokemon.Strategies;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;
using Pokedex.Framework.Patterns.Strategies;

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

    services.AddTransient<IStrategy<Pokemon, TranslatedDescription>, YodaPokemonDescriptionTranslationStrategy>();
    services.AddTransient<IStrategy<Pokemon, TranslatedDescription>, ShakespearePokemonDescriptionTranslationStrategy>();

    services.AddTransient(typeof(IStrategySelector<,>), typeof(StrategySelector<,>));

    services.AddTransient<IPokemonTranslator, PokemonTranslator>();

    return services;
  }
}
