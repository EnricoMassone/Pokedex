﻿using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Pokedex.Application.Translations.GetTranslatedPokemon;
using Pokedex.Application.Translations.GetTranslatedPokemon.Abstractions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.UnitTests.Translations.GetTranslatedPokemon;

public sealed class GetTranslatedPokemonQueryHandlerTests
{
  private readonly Mock<IPokemonRepository> _repositoryMock;
  private readonly Mock<IPokemonTranslator> _pokemonTranslatorMock;
  private readonly GetTranslatedPokemonQueryHandler _sut;

  public GetTranslatedPokemonQueryHandlerTests()
  {
    _repositoryMock = new Mock<IPokemonRepository>(MockBehavior.Strict);
    _pokemonTranslatorMock = new Mock<IPokemonTranslator>(MockBehavior.Strict);
    _sut = new GetTranslatedPokemonQueryHandler(_repositoryMock.Object, _pokemonTranslatorMock.Object);
  }

  [Theory]
  [AutoData]
  public async Task Handle_Throws_ArgumentNullException_When_Request_Is_Null(CancellationToken cancellationToken)
  {
    // ACT
    var exception = await Assert.ThrowsAsync<ArgumentNullException>(
      () => _sut.Handle(null!, cancellationToken)
    );

    // ASSERT
    exception.ParamName.Should().Be("request");
  }

  [Theory]
  [AutoData]
  public async Task Handle_Returns_Translated_Version_Of_Pokemon_Returned_By_Repository(
    Name name,
    Pokemon originalPokemon,
    Pokemon translatedPokemon,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    _repositoryMock
      .Setup(m => m.GetByNameAsync(name, cancellationToken))
      .ReturnsAsync(originalPokemon, TimeSpan.FromMilliseconds(value: 5));

    _pokemonTranslatorMock
      .Setup(m => m.TranslateAsync(originalPokemon, cancellationToken))
      .ReturnsAsync(translatedPokemon, TimeSpan.FromMilliseconds(value: 5));

    var query = new GetTranslatedPokemonQuery(name);

    // ACT
    var result = await _sut.Handle(query, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeTrue();

    // check result value
    var queryResponse = result.Value;

    queryResponse.Should().NotBeNull();
    queryResponse.Name.Should().Be(translatedPokemon.Name);
    queryResponse.IsLegendary.Should().Be(translatedPokemon.IsLegendary);
    queryResponse.Habitat.Should().Be(translatedPokemon.Habitat);
    queryResponse.Description.Should().Be(translatedPokemon.Description);
  }

  [Theory]
  [AutoData]
  public async Task Handle_Returns_Empty_Option_When_Repository_Returns_Empty_Option(
    Name name,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    _repositoryMock
      .Setup(m => m.GetByNameAsync(name, cancellationToken))
      .ReturnsAsync(Option<Pokemon>.None, TimeSpan.FromMilliseconds(value: 5));

    var query = new GetTranslatedPokemonQuery(name);

    // ACT
    var result = await _sut.Handle(query, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeFalse();
  }
}
