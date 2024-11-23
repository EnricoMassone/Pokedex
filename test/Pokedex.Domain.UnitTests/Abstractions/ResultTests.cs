using AutoFixture.Xunit2;
using FluentAssertions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Exceptions;

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

  [Theory]
  [AutoData]
  public void Ctor_Throws_ArgumentNullException_When_Error_Is_Null(TestData value)
  {
    // ACT
    var exception = Assert.Throws<ArgumentNullException>(
      () => new Result<TestData>(value, null!)
    );

    // ASSERT
    exception.ParamName.Should().Be("error");
  }

  [Theory]
  [AutoData]
  public void Creating_Result_With_Non_Null_Value_And_Error_Throws_InvalidResultArgumentsException(TestData value)
  {
    // ACT
    var exception = Assert.Throws<InvalidResultArgumentsException>(
      () => new Result<TestData>(value, TestDataErrors.TestError)
    );

    // ASSERT
    exception.Message.Should().Be("The provided combination of value and error parameters is invalid");
  }

  [Fact]
  public void Creating_Result_With_Null_Value_And_None_Error_Throws_InvalidResultArgumentsException()
  {
    // ACT
    var exception = Assert.Throws<InvalidResultArgumentsException>(
      () => new Result<TestData>(default, Error.None)
    );

    // ASSERT
    exception.Message.Should().Be("The provided combination of value and error parameters is invalid");
  }

  [Theory]
  [AutoData]
  public void Success_Factory_Method_Creates_Success_Result(TestData value)
  {
    // ACT
    var result = Result.Success(value);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
    result.IsFailure.Should().BeFalse();
    result.Error.Should().Be(Error.None);
    result.Value.Should().Be(value);
  }

  [Fact]
  public void Success_Factory_Method_Throws_ArgumentNullException_When_Value_Is_Null()
  {
    // ACT
    var exception = Assert.Throws<ArgumentNullException>(
      () => Result.Success<TestData>(null!)
    );

    // ASSERT
    exception.ParamName.Should().Be("value");
  }

  [Fact]
  public void Failure_Factory_Method_Creates_Failure_Result()
  {
    // ACT
    var result = Result.Failure<TestData>(TestDataErrors.TestError);

    // ASSERT
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeFalse();
    result.IsFailure.Should().BeTrue();
    result.Error.Should().Be(TestDataErrors.TestError);
  }

  [Fact]
  public void Failure_Factory_Method_Throws_ArgumentNullException_When_Error_Is_Null()
  {
    // ACT
    var exception = Assert.Throws<ArgumentNullException>(
      () => Result.Failure<TestData>(null!)
    );

    // ASSERT
    exception.ParamName.Should().Be("error");
  }

  [Fact]
  public void Failure_Factory_Method_Throws_ArgumentException_When_Error_Is_None_Error()
  {
    // ACT
    var exception = Assert.Throws<ArgumentException>(
      () => Result.Failure<TestData>(Error.None)
    );

    // ASSERT
    exception.ParamName.Should().Be("error");
    exception.Message.Should().Contain("Value of error parameter cannot be Error.None");
  }
}
