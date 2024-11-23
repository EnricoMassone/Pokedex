using FluentAssertions;
using Moq;
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
}
