using FluentAssertions;
using Pokedex.Domain.Pokemons;
using Pokedex.Infrastructure.PokeApis;
using RichardSzalay.MockHttp;
using System.Net;

namespace Pokedex.Infrastructure.UnitTests.PokeApis;

public sealed class PokeApiHttpClientTests
{
  [Fact]
  public async Task GetPokemonByNameAsync_Throws_ArgumentNullException_When_Name_Is_Null()
  {
    // ARRANGE
    var json = Helpers.ReadTestApiResponseFile(fileName: "PokemonSpeciesApiResponse.json");

    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.When(HttpMethod.Get, "https://server.example.com/api/v2/pokemon-species/*")
            .Respond("application/json", json);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new PokeApiHttpClient(httpClient);

    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => sut.GetPokemonByNameAsync(null!, CancellationToken.None)
    );

    // ASSERT
    exception.ParamName.Should().Be("name");
  }

  [Fact]
  public async Task GetPokemonByNameAsync_Returns_Non_Empty_Option_When_PokeApi_Returns_200()
  {
    // ARRANGE
    var json = Helpers.ReadTestApiResponseFile(fileName: "PokemonSpeciesApiResponse.json");

    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Get, "https://server.example.com/api/v2/pokemon-species/bulbasaur")
            .Respond("application/json", json);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new PokeApiHttpClient(httpClient);

    // ACT
    var result = await sut.GetPokemonByNameAsync(new Name("bulbasaur"), CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeTrue();

    // check result value
    var pokemon = result.Value;

    pokemon.Name.Should().Be("bulbasaur");

    pokemon.IsLegendary.Should().BeFalse();

    pokemon.Habitat.Should().NotBeNull();
    pokemon.Habitat!.Name.Should().Be("grassland");

    pokemon.FlavorTextEntries.Should().NotBeNull().And.HaveCount(2);
    pokemon.FlavorTextEntries[0].Should().NotBeNull();
    pokemon.FlavorTextEntries[0].FlavorText.Should().Be("A strange seed was\nplanted on its\nback at birth.\fThe plant sprouts\nand grows with\nthis POKéMON.");
    pokemon.FlavorTextEntries[0].Language.Should().NotBeNull();
    pokemon.FlavorTextEntries[0].Language.Name.Should().Be("en");
    pokemon.FlavorTextEntries[1].Should().NotBeNull();
    pokemon.FlavorTextEntries[1].FlavorText.Should().Be("A Bulbasaur es fácil verle echándose una siesta al sol.\nLa semilla que tiene en el lomo va creciendo cada vez más\na medida que absorbe los rayos del sol.");
    pokemon.FlavorTextEntries[1].Language.Should().NotBeNull();
    pokemon.FlavorTextEntries[1].Language.Name.Should().Be("es");

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }

  [Fact]
  public async Task GetPokemonByNameAsync_Returns_Empty_Option_When_PokeApi_Returns_404()
  {
    // ARRANGE
    using var mockHttp = new MockHttpMessageHandler();

    mockHttp.Expect(HttpMethod.Get, "https://server.example.com/api/v2/pokemon-species/bulbasaur")
            .Respond(HttpStatusCode.NotFound);

    using var httpClient = mockHttp.ToHttpClient();

    httpClient.BaseAddress = new Uri("https://server.example.com/");

    var sut = new PokeApiHttpClient(httpClient);

    // ACT
    var result = await sut.GetPokemonByNameAsync(new Name("bulbasaur"), CancellationToken.None);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeFalse();

    // check executed HTTP requests
    mockHttp.VerifyNoOutstandingExpectation();
  }
}
