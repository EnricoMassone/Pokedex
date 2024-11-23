using MediatR;
using Pokedex.Application.Extensions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Pokemons.GetPokemon;

public sealed class GetPokemonQueryHandler : IRequestHandler<GetPokemonQuery, Option<GetPokemonQueryResponse>>
{
  private readonly IPokemonRepository _repository;

  public GetPokemonQueryHandler(IPokemonRepository repository)
  {
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
  }

  public async Task<Option<GetPokemonQueryResponse>> Handle(GetPokemonQuery request, CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(request);

    var optionalPokemon = await _repository.GetByNameAsync(request.Name, cancellationToken).ConfigureAwait(false);

    if (!optionalPokemon.HasValue)
    {
      return Option<GetPokemonQueryResponse>.None;
    }

    return optionalPokemon.Value.ToGetPokemonQueryResponse();
  }
}
