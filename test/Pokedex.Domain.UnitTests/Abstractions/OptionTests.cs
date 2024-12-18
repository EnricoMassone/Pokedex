﻿using AutoFixture.Xunit2;
using FluentAssertions;
using Pokedex.Domain.Abstractions;
using Pokedex.Domain.Exceptions;

namespace Pokedex.Domain.UnitTests.Abstractions;

public sealed class OptionTests
{
  [Theory]
  [AutoData]
  public void Non_Empty_Option_Can_Be_Created(TestData value)
  {
    // ACT
    var result = new Option<TestData>(value, hasValue: true);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeTrue();
    result.Value.Should().Be(value);
  }

  [Fact]
  public void Empty_Option_Can_Be_Created()
  {
    // ACT
    var result = new Option<TestData>(default, hasValue: false);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeFalse();
  }

  [Fact]
  public void None_Field_Contains_Empty_Option()
  {
    // ACT
    var result = Option<TestData>.None;

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeFalse();
  }

  [Theory]
  [AutoData]
  public void Non_Null_Value_Can_Be_Implicitly_Converted_To_Non_Empty_Option(TestData value)
  {
    // ACT
    Option<TestData> result = value;

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeTrue();
    result.Value.Should().Be(value);
  }

  [Fact]
  public void Null_Value_Can_Be_Implicitly_Converted_To_Empty_Option()
  {
    // ACT
    Option<TestData> result = (TestData?)null;

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeFalse();
  }

  [Theory]
  [AutoData]
  public void Value_Of_Non_Empty_Option_Can_Be_Read(TestData value)
  {
    // ACT
    var result = new Option<TestData>(value, hasValue: true);

    // ASSERT
    result.Value.Should().Be(value);
  }

  [Theory]
  [AutoData]
  public void Creating_Option_With_Non_Null_Value_Flagged_As_Empty_Throws_InvalidOptionArgumentsException(TestData value)
  {
    // ACT
    var exception = Assert.Throws<InvalidOptionArgumentsException>(
      () => new Option<TestData>(value, hasValue: false)
    );

    // ASSERT
    exception.Message.Should().Be("The provided combination of value and hasValue parameters is invalid");
  }

  [Fact]
  public void Creating_Option_With_Null_Value_Flagged_As_Non_Empty_Throws_InvalidOptionArgumentsException()
  {
    // ACT
    var exception = Assert.Throws<InvalidOptionArgumentsException>(
      () => new Option<TestData>(default, hasValue: true)
    );

    // ASSERT
    exception.Message.Should().Be("The provided combination of value and hasValue parameters is invalid");
  }

  [Fact]
  public void Reading_Value_Of_Empty_Option_Throws_InvalidOperationException()
  {
    // ARRANGE
    var sut = new Option<TestData>(default, hasValue: false);

    // ACT
    var exception = Assert.Throws<InvalidOperationException>(() => sut.Value);

    // ASSERT
    exception.Message.Should().Be("Value of an empty option cannot be accessed");
  }

  [Theory]
  [AutoData]
  public void Some_Factory_Method_Creates_Non_Empty_Option(TestData value)
  {
    // ACT
    var result = Option.Some(value);

    // ASSERT
    result.Should().NotBeNull();
    result.HasValue.Should().BeTrue();
    result.Value.Should().Be(value);
  }

  [Fact]
  public void Some_Factory_Method_Throws_ArgumentNullException_When_Provided_Value_Is_Null()
  {
    // ACT
    var exception = Assert.Throws<ArgumentNullException>(
      () => Option.Some(default(TestData)!)
    );

    // ASSERT
    exception.ParamName.Should().Be("value");
  }
}
