using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Pokedex.Framework.Patterns.Strategies;
using Pokedex.Framework.Patterns.Strategies.Exceptions;

namespace Pokedex.Framework.UnitTests.Patterns.Strategies;

public sealed class StrategySelectorTests
{
  [Theory]
  [AutoData]
  public void GetRequiredMatchingStrategy_Returns_Matching_Strategy_When_Exists(FakeData data)
  {
    // ARRANGE
    var strategy1Mock = new Mock<IStrategy<FakeData, FakeResult>>(MockBehavior.Strict);
    var strategy2Mock = new Mock<IStrategy<FakeData, FakeResult>>(MockBehavior.Strict);

    strategy1Mock
      .Setup(m => m.CanHandle(It.IsAny<FakeData>()))
      .Returns(false);

    strategy2Mock
      .Setup(m => m.CanHandle(It.IsAny<FakeData>()))
      .Returns(true);

    var sut = new StrategySelector<FakeData, FakeResult>([strategy1Mock.Object, strategy2Mock.Object]);

    // ACT
    var result = sut.GetRequiredMatchingStrategy(data);

    // ASSERT
    result.Should().BeSameAs(strategy2Mock.Object);
  }

  [Theory]
  [AutoData]
  public void GetRequiredMatchingStrategy_Returns_First_Matching_Strategy_When_More_Than_One_Exist(FakeData data)
  {
    // ARRANGE
    var strategy1Mock = new Mock<IStrategy<FakeData, FakeResult>>(MockBehavior.Strict);
    var strategy2Mock = new Mock<IStrategy<FakeData, FakeResult>>(MockBehavior.Strict);
    var strategy3Mock = new Mock<IStrategy<FakeData, FakeResult>>(MockBehavior.Strict);

    strategy1Mock
      .Setup(m => m.CanHandle(It.IsAny<FakeData>()))
      .Returns(false);

    strategy2Mock
      .Setup(m => m.CanHandle(It.IsAny<FakeData>()))
      .Returns(true);

    strategy3Mock
      .Setup(m => m.CanHandle(It.IsAny<FakeData>()))
      .Returns(true);

    var sut = new StrategySelector<FakeData, FakeResult>(
      [strategy1Mock.Object, strategy2Mock.Object, strategy3Mock.Object]
    );

    // ACT
    var result = sut.GetRequiredMatchingStrategy(data);

    // ASSERT
    result.Should().BeSameAs(strategy2Mock.Object);
  }

  [Theory]
  [AutoData]
  public void GetRequiredMatchingStrategy_Throws_MissingMatchingStrategyException_When_No_Matching_Strategy_Exists(
    FakeData data)
  {
    // ARRANGE
    var strategy1Mock = new Mock<IStrategy<FakeData, FakeResult>>(MockBehavior.Strict);
    var strategy2Mock = new Mock<IStrategy<FakeData, FakeResult>>(MockBehavior.Strict);

    strategy1Mock
      .Setup(m => m.CanHandle(It.IsAny<FakeData>()))
      .Returns(false);

    strategy2Mock
      .Setup(m => m.CanHandle(It.IsAny<FakeData>()))
      .Returns(false);

    var sut = new StrategySelector<FakeData, FakeResult>([strategy1Mock.Object, strategy2Mock.Object]);

    // ACT
    var exception = Assert.Throws<MissingMatchingStrategyException>(
      () => sut.GetRequiredMatchingStrategy(data)
    );

    // ASSERT
    exception.Message.Should().Be("There is no matching strategy for the provided data of type FakeData");
  }
}

public sealed record FakeData(string Name);
public sealed record FakeResult(string Name);
