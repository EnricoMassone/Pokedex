using AutoFixture.Xunit2;

namespace Pokedex.Infrastructure.UnitTests.PokeApis;

public sealed class PokeApiHttpClientTests
{
  [Theory]
  [AutoData]
  public async Task GetPokemonByNameAsync_Throws_ArgumentNullException_When_Name_Is_Null(
    CancellationToken cancellationToken)
  {

  }
}
