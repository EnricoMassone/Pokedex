using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Application.Translations.GetTranslatedPokemon.Services;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;
using Pokedex.Framework.Patterns.Strategies;

namespace Pokedex.Application.UnitTests.Translations.GetTranslatedPokemon.Services;

public sealed class PokemonTranslatorTests
{
  private readonly Mock<IStrategySelector<Pokemon, TranslatedDescription>> _strategySelectorMock;
  private readonly Mock<ILogger<PokemonTranslator>> _loggerMock;
  private readonly PokemonTranslator _sut;

  public PokemonTranslatorTests()
  {
    _strategySelectorMock = new Mock<IStrategySelector<Pokemon, TranslatedDescription>>(MockBehavior.Strict);
    _loggerMock = new Mock<ILogger<PokemonTranslator>>(MockBehavior.Loose);

    _sut = new PokemonTranslator(
      _strategySelectorMock.Object,
      _loggerMock.Object);
  }

  [Theory]
  [AutoData]
  public async Task TranslateAsync_Throws_ArgumentNullException_When_Pokemon_Is_Null(
    CancellationToken cancellationToken)
  {
    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => _sut.TranslateAsync(null!, cancellationToken)
    );

    // ASSERT
    exception.ParamName.Should().Be("pokemon");
  }

  [Theory]
  [AutoData]
  public async Task TranslateAsync_Translates_Poken_Description_Using_Strategy_Selected_By_Strategy_Selector(
    Pokemon pokemon,
    TranslatedDescription translatedDescription,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var translationStrategyMock = new Mock<IStrategy<Pokemon, TranslatedDescription>>(MockBehavior.Strict);

    translationStrategyMock
      .Setup(m => m.HandleAsync(pokemon, cancellationToken))
      .ReturnsAsync(translatedDescription, TimeSpan.FromMilliseconds(5));

    _strategySelectorMock
      .Setup(m => m.GetRequiredMatchingStrategy(pokemon))
      .Returns(translationStrategyMock.Object);

    // ACT
    var result = await _sut.TranslateAsync(pokemon, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Name.Should().Be(pokemon.Name);
    result.IsLegendary.Should().Be(pokemon.IsLegendary);
    result.Habitat.Should().Be(pokemon.Habitat);
    result.Description.Should().Be(translatedDescription);

    // check logger calls

    _loggerMock.VerifyLog(logger =>
      logger.LogInformation(
        "Successfully translated Pokemon description using strategy {Strategy}",
        translationStrategyMock.Object.GetType().Name
      ), Times.Once()
    );

    _loggerMock.VerifyNoOtherCalls();
  }
}
