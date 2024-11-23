using AutoFixture.Xunit2;
using FluentAssertions;
using Pokedex.Domain.Pokemons;

namespace Pokedex.Domain.UnitTests.Pokemons;

public sealed class NameTests
{
  [Theory]
  [AutoData]
  public void Ctor_Creates_New_Instance(string value)
  {
    // ACT
    var result = new Name(value);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().Be(value);
  }

  [Theory]
  [AutoData]
  public void Implicit_Cast_To_String_Works(string value)
  {
    // ARRANGE
    var sut = new Name(value);

    // ACT
    string result = sut;

    // ASSERT
    result.Should().Be(value);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("    ")]
  public void Ctor_Throws_ArgumentException_When_Value_Is_Null_Or_White_Space(string? value)
  {
    // ACT
    var exception = Assert.Throws<ArgumentException>(
      () => new Name(value!)
    );

    // ASSERT
    exception.ParamName.Should().Be("value");
    exception.Message.Should().Contain("The value of a name cannot be null or white space");
  }
}
