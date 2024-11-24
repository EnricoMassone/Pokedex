using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Application.Translations.Abstractions;
using Pokedex.Application.Translations.Strategies;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.UnitTests.Translations.Strategies;

public sealed class YodaPokemonDescriptionTranslationStrategyTests
{
  private readonly Mock<IFunTranslationsApiHttpClient> _httpClientMock;
  private readonly Mock<ILogger<YodaPokemonDescriptionTranslationStrategy>> _loggerMock;
  private readonly YodaPokemonDescriptionTranslationStrategy _sut;

  public YodaPokemonDescriptionTranslationStrategyTests()
  {
    _httpClientMock = new Mock<IFunTranslationsApiHttpClient>(MockBehavior.Strict);
    _loggerMock = new Mock<ILogger<YodaPokemonDescriptionTranslationStrategy>>(MockBehavior.Strict);

    _sut = new YodaPokemonDescriptionTranslationStrategy(
      _httpClientMock.Object,
      _loggerMock.Object);
  }

  [Fact]
  public void CanHandle_Throws_ArgumentNullException_When_Data_Is_Null()
  {
    // ACT
    var exception = Assert.Throws<ArgumentNullException>(
      () => _sut.CanHandle(null!)
    );

    // ASSERT
    exception.ParamName.Should().Be("data");
  }

  [Theory]
  [AutoData]
  public void CanHandle_Returns_True_When_Pokemon_Habitat_Is_Cave(Pokemon pokemon)
  {
    // ARRANGE
    var data = pokemon with { Habitat = PokemonHabitats.Cave, IsLegendary = false };

    // ACT
    var result = _sut.CanHandle(data);

    // ASSERT
    result.Should().BeTrue();
  }

  [Theory]
  [AutoData]
  public void CanHandle_Returns_True_When_Pokemon_Is_Legendary(Pokemon pokemon)
  {
    // ARRANGE
    var data = pokemon with { Habitat = "forest", IsLegendary = true };

    // ACT
    var result = _sut.CanHandle(data);

    // ASSERT
    result.Should().BeTrue();
  }

  [Theory]
  [AutoData]
  public void CanHandle_Returns_True_When_Pokemon_Habitat_Is_Cave_And_Pokemon_Is_Legendary(Pokemon pokemon)
  {
    // ARRANGE
    var data = pokemon with { Habitat = PokemonHabitats.Cave, IsLegendary = true };

    // ACT
    var result = _sut.CanHandle(data);

    // ASSERT
    result.Should().BeTrue();
  }

  [Theory]
  [AutoData]
  public void CanHandle_Returns_False_When_Pokemon_Habitat_Is_Not_Cave_And_Pokemon_Is_Not_Legendary(Pokemon pokemon)
  {
    // ARRANGE
    var data = pokemon with { Habitat = "forest", IsLegendary = false };

    // ACT
    var result = _sut.CanHandle(data);

    // ASSERT
    result.Should().BeFalse();
  }
}
