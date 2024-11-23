﻿using Pokedex.Domain.Abstractions;
using System.Net;
using System.Net.Http.Json;

namespace Pokedex.Infrastructure.PokemonApis;

public sealed class PokeApiHttpClient : IPokeApiHttpClient
{
  private readonly HttpClient _httpClient;

  public PokeApiHttpClient(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
  }

  public async Task<Option<PokemonApiResponse>> GetPokemonByNameAsync(string name, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("Pokemon name cannot be null or white space", nameof(name));
    }

    var requestUri = BuildGetPokemonSpeciesApiUri(name);

    PokemonApiResponse? response;

    try
    {
      response = await _httpClient.GetFromJsonAsync<PokemonApiResponse>(
        requestUri,
        cancellationToken).ConfigureAwait(false);
    }
    catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
    {
      return Option<PokemonApiResponse>.None;
    }

    return response;
  }

  private static Uri BuildGetPokemonSpeciesApiUri(string pokemonName) =>
    new($"api/v2/pokemon-species/{pokemonName}", UriKind.Relative);
}
