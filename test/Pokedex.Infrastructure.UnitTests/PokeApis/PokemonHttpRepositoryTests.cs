using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;
using Pokedex.Infrastructure.PokeApis;

namespace Pokedex.Infrastructure.UnitTests.PokeApis;

public sealed class PokemonHttpRepositoryTests
{
  private readonly Mock<IPokeApiHttpClient> _httpClientMock;
  private readonly PokemonHttpRepository _sut;

  public PokemonHttpRepositoryTests()
  {
    _httpClientMock = new Mock<IPokeApiHttpClient>(MockBehavior.Strict);
    _sut = new PokemonHttpRepository(_httpClientMock.Object);
  }

  [Fact]
  public async Task GetPokemonByNameAsync_Throws_ArgumentNullException_When_Name_Is_Null()
  {
    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => _sut.GetByNameAsync(null!, CancellationToken.None)
    );

    // ASSERT
    exception.ParamName.Should().Be("name");
  }

  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Returns_Pokemon_Based_On_Http_Client_Response(
    string name,
    bool isLegendary,
    string habitat,
    string flavorText,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var habitatDto = new HabitatDto { Name = habitat };

    var flavorTextDto = new FlavorTextDto
    {
      FlavorText = flavorText,
      Language = new LanguageDto { Name = CultureCodes.English },
    };

    var pokemonApiResponse = new PokemonApiResponse
    {
      IsLegendary = isLegendary,
      Name = name,
      FlavorTextEntries = [flavorTextDto],
      Habitat = habitatDto,
    };

    _httpClientMock
      .Setup(m => m.GetPokemonByNameAsync(new Name(name), cancellationToken))
      .ReturnsAsync(pokemonApiResponse, TimeSpan.FromMilliseconds(value: 10));

    // ACT
    var result = await _sut.GetByNameAsync(new Name(name), cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeTrue();

    // check result value
    var pokemon = result.Value;

    pokemon.Should().NotBeNull();
    pokemon.Name.Should().Be(new Name(name));
    pokemon.IsLegendary.Should().Be(isLegendary);
    pokemon.Habitat.Should().Be(habitat);
    pokemon.Description.Should().Be(flavorText);
  }

  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Returns_Empty_Option_When_Http_Client_Returns_Empty_Option(
    string name,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    _httpClientMock
      .Setup(m => m.GetPokemonByNameAsync(new Name(name), cancellationToken))
      .ReturnsAsync(Option<PokemonApiResponse>.None, TimeSpan.FromMilliseconds(value: 10));

    // ACT
    var result = await _sut.GetByNameAsync(new Name(name), cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeFalse();
  }
}
