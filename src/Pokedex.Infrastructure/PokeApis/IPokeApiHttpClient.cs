﻿using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Infrastructure.PokeApis;

public interface IPokeApiHttpClient
{
  Task<Option<PokemonApiResponse>> GetPokemonByNameAsync(Name name, CancellationToken cancellationToken);
}