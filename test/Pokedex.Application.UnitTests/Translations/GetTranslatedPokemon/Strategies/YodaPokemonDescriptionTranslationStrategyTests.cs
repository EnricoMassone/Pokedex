using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;
using Pokedex.Application.Translations.GetTranslatedPokemon.Strategies;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.UnitTests.Translations.GetTranslatedPokemon.Strategies;

public sealed class YodaPokemonDescriptionTranslationStrategyTests
{
  private readonly Mock<IFunTranslationsApiHttpClient> _httpClientMock;
  private readonly Mock<ILogger<YodaPokemonDescriptionTranslationStrategy>> _loggerMock;
  private readonly YodaPokemonDescriptionTranslationStrategy _sut;

  public YodaPokemonDescriptionTranslationStrategyTests()
  {
    _httpClientMock = new Mock<IFunTranslationsApiHttpClient>(MockBehavior.Loose);
    _loggerMock = new Mock<ILogger<YodaPokemonDescriptionTranslationStrategy>>(MockBehavior.Loose);

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

  [Theory]
  [AutoData]
  public async Task HandleAsync_Throws_ArgumentNullException_When_Data_Is_Null(
    CancellationToken cancellationToken)
  {
    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => _sut.HandleAsync(null!, cancellationToken)
    );

    // ASSERT
    exception.ParamName.Should().Be("data");
  }

  [Theory]
  [AutoData]
  public async Task HandleAsync_Throws_InvalidOperationException_When_Data_Cannot_Be_Handled(
    Pokemon pokemon,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var data = pokemon with { Habitat = "forest", IsLegendary = false };

    // ACT
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(
      () => _sut.HandleAsync(data, cancellationToken)
    );

    // ASSERT
    exception.Message.Should().Be("Strategy YodaPokemonDescriptionTranslationStrategy cannot handle the provided data of type Pokemon");
  }

  [Theory]
  [InlineAutoData(null)]
  [InlineAutoData("")]
  [InlineAutoData("   ")]
  public async Task HandleAsync_Does_Not_Translate_Pokemon_Description_If_It_Is_Null_Or_White_Space(
    string? originalDescription,
    Pokemon pokemon,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var data = pokemon with { IsLegendary = true, Description = originalDescription };

    // ACT
    var result = await _sut.HandleAsync(data, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().Be(originalDescription);

    // check mock calls
    _httpClientMock.VerifyNoOtherCalls();
    _loggerMock.VerifyNoOtherCalls();
  }

  [Theory]
  [AutoData]
  public async Task HandleAsync_Translates_Pokemon_Description_Using_Yoda_Translation_Endpoint(
    Pokemon pokemon,
    string originalDescription,
    FunTranslationsApiResponse funTranslationsApiResponse,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var data = pokemon with { IsLegendary = true, Description = originalDescription };

    _httpClientMock
      .Setup(m => m.ApplyYodaTranslationAsync(It.IsAny<Text>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(Result.Success(funTranslationsApiResponse), TimeSpan.FromMilliseconds(5));

    // ACT
    var result = await _sut.HandleAsync(data, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().Be(funTranslationsApiResponse.Contents.Translated);

    // check mock calls
    _httpClientMock
      .Verify(
        m => m.ApplyYodaTranslationAsync(new Text(originalDescription), cancellationToken),
        Times.Once()
      );

    _httpClientMock.VerifyNoOtherCalls();

    _loggerMock.VerifyNoOtherCalls();
  }

  [Theory]
  [AutoData]
  public async Task HandleAsync_Returns_Original_Pokemon_Description_When_Yoda_Translation_Endpoint_Fails(
    Pokemon pokemon,
    string originalDescription,
    Error error,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var data = pokemon with { IsLegendary = true, Description = originalDescription };

    _httpClientMock
      .Setup(m => m.ApplyYodaTranslationAsync(It.IsAny<Text>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(Result.Failure<FunTranslationsApiResponse>(error), TimeSpan.FromMilliseconds(5));

    // ACT
    var result = await _sut.HandleAsync(data, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().Be(originalDescription);

    // check mock calls
    _httpClientMock
      .Verify(
        m => m.ApplyYodaTranslationAsync(new Text(originalDescription), cancellationToken),
        Times.Once()
      );

    _httpClientMock.VerifyNoOtherCalls();

    _loggerMock.VerifyLog(
      logger => logger.LogWarning(
        "Yoda translation of Pokemon description failed with error code {ErrorCode}. Reason: {Reason}",
        error.Code,
        error.Description), Times.Once()
    );

    _loggerMock.VerifyNoOtherCalls();
  }
}
