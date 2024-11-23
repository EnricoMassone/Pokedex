using Pokedex.Domain.Abstractions;

namespace Pokedex.Domain.UnitTests.Abstractions;

public sealed class TestData
{
  public required string Foo { get; init; }
}

public static class TestDataErrors
{
  public static readonly Error TestError = new(Code: "TestData.TestError", Description: "Just a fake error for unit tests");
}
