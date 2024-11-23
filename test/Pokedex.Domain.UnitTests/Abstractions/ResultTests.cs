using AutoFixture.Xunit2;
using FluentAssertions;
using Pokedex.Domain.Abstractions;

namespace Pokedex.Domain.UnitTests.Abstractions;

public sealed class ResultTests
{
  [Theory]
  [AutoData]
  public void Success_Result_Can_Be_Created(TestData value)
  {
    // ACT
    var result = new Result<TestData>(value, Error.None);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
    result.IsFailure.Should().BeFalse();
    result.Error.Should().Be(Error.None);
    result.Value.Should().Be(value);
  }

  [Fact]
  public void Failure_Result_Can_Be_Created()
  {
    // ACT
    var result = new Result<TestData>(default, TestDataErrors.TestError);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeFalse();
    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be(TestDataErrors.TestError);
  }

  [Theory]
  [AutoData]
  public void Value_Of_Success_Result_Can_Be_Read(TestData value)
  {
    // ACT
    var result = new Result<TestData>(value, Error.None);

    // ASSERT
    result.Value.Should().Be(value);
  }

  [Fact]
  public void Reading_Value_Of_Failure_Result_Throws_InvalidOperationException()
  {
    // ARRANGE
    var sut = new Result<TestData>(default, TestDataErrors.TestError);

    // ACT
    var exception = Assert.Throws<InvalidOperationException>(() => sut.Value);

    // ASSERT
    exception.Message.Should().Be("Value of a failure result cannot be accessed");
  }
}
