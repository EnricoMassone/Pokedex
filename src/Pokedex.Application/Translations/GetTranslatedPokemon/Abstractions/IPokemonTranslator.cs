using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;

public interface IPokemonTranslator
{
  Task<Pokemon> TranslateAsync(Pokemon pokemon, CancellationToken cancellationToken);
}
