using Pokedex.Domain.Abstractions;

namespace Pokedex.Domain.Pokemons;

public interface IPokemonRepository
{
  Task<Option<Pokemon>> GetByNameAsync(string name, CancellationToken cancellationToken);
}
