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

  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Throws_ArgumentNullException_When_Name_Is_Null(CancellationToken cancellationToken)
  {
    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => _sut.GetByNameAsync(null!, cancellationToken)
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

  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Handles_Null_Value_Of_Habitat_Property(
    string name,
    bool isLegendary,
    string flavorText,
    CancellationToken cancellationToken)
  {
    // ARRANGE
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
      Habitat = null,
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
    pokemon.Habitat.Should().BeNull();
  }

  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Handles_Empty_List_Of_FlavorTextEntries(
    string name,
    bool isLegendary,
    string habitat,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var habitatDto = new HabitatDto { Name = habitat };

    var pokemonApiResponse = new PokemonApiResponse
    {
      IsLegendary = isLegendary,
      Name = name,
      FlavorTextEntries = [],
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
    pokemon.Description.Should().BeNull();
  }

  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Handles_FlavorTextEntries_List_With_Languages_Other_Than_English(
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
      Language = new LanguageDto { Name = "it" },
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
    pokemon.Description.Should().BeNull();
  }

  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Set_Description_Value_Using_First_Availabler_English_FlavorText(
    string name,
    bool isLegendary,
    string habitat,
    string flavorText1,
    string flavorText2,
    string flavorText3,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var habitatDto = new HabitatDto { Name = habitat };

    var flavorTextDto1 = new FlavorTextDto
    {
      FlavorText = flavorText1,
      Language = new LanguageDto { Name = "it" },
    };

    var flavorTextDto2 = new FlavorTextDto
    {
      FlavorText = flavorText2,
      Language = new LanguageDto { Name = CultureCodes.English },
    };

    var flavorTextDto3 = new FlavorTextDto
    {
      FlavorText = flavorText3,
      Language = new LanguageDto { Name = CultureCodes.English },
    };

    var pokemonApiResponse = new PokemonApiResponse
    {
      IsLegendary = isLegendary,
      Name = name,
      FlavorTextEntries = [flavorTextDto1, flavorTextDto2, flavorTextDto3],
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
    pokemon.Description.Should().Be(flavorText2);
  }
}
