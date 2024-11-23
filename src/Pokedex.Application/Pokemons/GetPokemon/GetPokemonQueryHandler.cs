using MediatR;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Pokemons.GetPokemon;

public sealed class GetPokemonQueryHandler : IRequestHandler<GetPokemonQuery, Option<Pokemon>>
{
  private readonly IPokemonRepository _repository;

  public GetPokemonQueryHandler(IPokemonRepository repository)
  {
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
  }

  public async Task<Option<Pokemon>> Handle(GetPokemonQuery request, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(request);

    var response = await _repository.GetByNameAsync(request.Name, cancellationToken).ConfigureAwait(false);

    return response;
  }
}
