using MediatR;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Translations.GetTranslatedPokemon;

public sealed record GetTranslatedPokemonQuery(Name Name) : IRequest<Option<GetTranslatedPokemonQueryResponse>>;
