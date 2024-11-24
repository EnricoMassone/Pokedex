using AutoFixture.Xunit2;
using FluentAssertions;
using Pokedex.Domain.Abstractions;

namespace Pokedex.Domain.UnitTests.Abstractions;

public sealed class TextTests
{
  [Theory]
  [AutoData]
  public void Ctor_Creates_New_Instance(string value)
  {
    // ACT
    var result = new Text(value);

    // ASSERT
    result.Should().NotBeNull();
    result.Value.Should().Be(value);
  }

  [Theory]
  [AutoData]
  public void Implicit_Cast_To_String_Works(string value)
  {
    // ARRANGE
    var sut = new Text(value);

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
      () => new Text(value!)
    );

    // ASSERT
    exception.ParamName.Should().Be("value");
    exception.Message.Should().Contain("The value of a text cannot be null or white space");
  }
}
