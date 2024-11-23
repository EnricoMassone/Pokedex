using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Infrastructure.PokemonApis;

public sealed class PokemonHttpRepository : IPokemonRepository
{
  public Task<Option<Pokemon>> GetByNameAsync(string name, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
