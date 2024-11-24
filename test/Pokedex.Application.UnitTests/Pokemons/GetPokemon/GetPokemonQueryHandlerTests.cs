using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Pokedex.Application.Pokemons.GetPokemon;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Application.UnitTests.Pokemons.GetPokemon;

public sealed class GetPokemonQueryHandlerTests
{
  private readonly Mock<IPokemonRepository> _pokemonRepositoryMock;
  private readonly GetPokemonQueryHandler _sut;

  public GetPokemonQueryHandlerTests()
  {
    _pokemonRepositoryMock = new Mock<IPokemonRepository>(MockBehavior.Strict);
    _sut = new GetPokemonQueryHandler(_pokemonRepositoryMock.Object);
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
  public async Task Handle_Returns_Response_Based_On_Pokemon_Returned_By_Repository(
    Name name,
    Pokemon pokemon,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    _pokemonRepositoryMock
      .Setup(m => m.GetByNameAsync(name, cancellationToken))
      .ReturnsAsync(pokemon, TimeSpan.FromMilliseconds(value: 5));

    var query = new GetPokemonQuery(name);

    // ACT
    var result = await _sut.Handle(query, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeTrue();

    // check result value
    var queryResponse = result.Value;

    queryResponse.Should().NotBeNull();
    queryResponse.Name.Should().Be(pokemon.Name);
    queryResponse.IsLegendary.Should().Be(pokemon.IsLegendary);
    queryResponse.Habitat.Should().Be(pokemon.Habitat);
    queryResponse.Description.Should().Be(pokemon.Description);
  }

  [Theory]
  [AutoData]
  public async Task Handle_Returns_Empty_Option_When_Repository_Returns_Empty_Option(
    Name name,
    CancellationToken cancellationToken)
  {
    // ARRANGE
    _pokemonRepositoryMock
      .Setup(m => m.GetByNameAsync(name, cancellationToken))
      .ReturnsAsync(Option<Pokemon>.None, TimeSpan.FromMilliseconds(value: 5));

    var query = new GetPokemonQuery(name);

    // ACT
    var result = await _sut.Handle(query, cancellationToken);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeFalse();
  }
}
