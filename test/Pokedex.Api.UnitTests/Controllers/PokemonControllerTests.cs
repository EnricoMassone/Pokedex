using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pokedex.Api.Controllers;
using Pokedex.Application.Pokemons.GetPokemon;
using Pokedex.Application.Translations.GetTranslatedPokemon;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Api.UnitTests.Controllers;

public sealed class PokemonControllerTests
{
  private readonly Mock<ISender> _senderMock;
  private readonly PokemonController _sut;

  public PokemonControllerTests()
  {
    _senderMock = new Mock<ISender>(MockBehavior.Strict);
    _sut = new PokemonController(_senderMock.Object);
  }

  [Theory]
  [AutoData]
  public async Task GetByName_Returns_Pokemon_Value_When_Query_Returns_Non_Empty_Option(
    Name name,
    GetPokemonQueryResponse response,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var query = new GetPokemonQuery(name);

    _senderMock
      .Setup(m => m.Send(query, cancellationToken))
      .ReturnsAsync(response, TimeSpan.FromMilliseconds(value: 5));

    // ACT
    var result = await _sut.GetByName(name, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().NotBeNull();
    result.Value.Should().Be(response);
    result.Result.Should().BeNull();
  }

  [Theory]
  [AutoData]
  public async Task GetByName_Returns_NotFound_Action_Result_When_Query_Returns_Empty_Option(
    Name name,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var query = new GetPokemonQuery(name);

    _senderMock
      .Setup(m => m.Send(query, cancellationToken))
      .ReturnsAsync(Option<GetPokemonQueryResponse>.None, TimeSpan.FromMilliseconds(value: 5));

    // ACT
    var result = await _sut.GetByName(name, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().BeNull();
    result.Result.Should().NotBeNull();
    result.Result.Should().BeOfType<NotFoundResult>();
  }

  [Theory]
  [AutoData]
  public async Task GetTranslatedByName_Returns_Pokemon_Value_When_Query_Returns_Non_Empty_Option(
    Name name,
    GetTranslatedPokemonQueryResponse response,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var query = new GetTranslatedPokemonQuery(name);

    _senderMock
      .Setup(m => m.Send(query, cancellationToken))
      .ReturnsAsync(response, TimeSpan.FromMilliseconds(value: 5));

    // ACT
    var result = await _sut.GetTranslatedByName(name, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().NotBeNull();
    result.Value.Should().Be(response);
    result.Result.Should().BeNull();
  }

  [Theory]
  [AutoData]
  public async Task GetTranslatedByName_Returns_NotFound_Action_Result_When_Query_Returns_Empty_Option(
    Name name,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    var query = new GetTranslatedPokemonQuery(name);

    _senderMock
      .Setup(m => m.Send(query, cancellationToken))
      .ReturnsAsync(Option<GetTranslatedPokemonQueryResponse>.None, TimeSpan.FromMilliseconds(value: 5));

    // ACT
    var result = await _sut.GetTranslatedByName(name, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().BeNull();
    result.Result.Should().NotBeNull();
    result.Result.Should().BeOfType<NotFoundResult>();
  }
}
