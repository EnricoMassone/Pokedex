using MediatR;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.Pokemons.GetPokemon;

public sealed record GetPokemonQuery(Name Name) : IRequest<Option<GetPokemonQueryResponse>>;
