using MediatR;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Pokemons.GetPokemon;

public sealed class GetPokemonQueryHandler : IRequestHandler<GetPokemonQuery, Option<Pokemon>>
{
  public Task<Option<Pokemon>> Handle(GetPokemonQuery request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
