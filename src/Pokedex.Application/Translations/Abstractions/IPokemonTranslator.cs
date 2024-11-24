using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Translations.Abstractions;

public interface IPokemonTranslator
{
  Task<Pokemon> TranslateAsync(Pokemon pokemon, CancellationToken cancellationToken);
}
