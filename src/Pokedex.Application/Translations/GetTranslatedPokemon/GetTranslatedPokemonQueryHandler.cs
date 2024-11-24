using MediatR;
using Pokedex.Application.Extensions;
using Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Translations.GetTranslatedPokemon;

public sealed class GetTranslatedPokemonQueryHandler : IRequestHandler<GetTranslatedPokemonQuery, Option<GetTranslatedPokemonQueryResponse>>
{
  private readonly IPokemonRepository _repository;
  private readonly IPokemonTranslator _pokemonTranslator;

  public GetTranslatedPokemonQueryHandler(IPokemonRepository repository, IPokemonTranslator pokemonTranslator)
  {
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    _pokemonTranslator = pokemonTranslator ?? throw new ArgumentNullException(nameof(pokemonTranslator));
  }

  public async Task<Option<GetTranslatedPokemonQueryResponse>> Handle(
    GetTranslatedPokemonQuery request,
    CancellationToken cancellationToken)
  {
    ArgumentNullException.ThrowIfNull(request);

    var optionalPokemon = await _repository.GetByNameAsync(
      request.Name,
      cancellationToken).ConfigureAwait(false);

    if (!optionalPokemon.HasValue)
    {
      return Option<GetTranslatedPokemonQueryResponse>.None;
    }

    var translatedPokemon = await _pokemonTranslator.TranslateAsync(
      optionalPokemon.Value,
      cancellationToken).ConfigureAwait(false);

    return translatedPokemon.ToGetTranslatedPokemonQueryResponse();
  }
}
