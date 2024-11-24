namespace Pokedex.Framework.Patterns.Strategies.Exceptions;

public class MissingMatchingStrategyException : Exception
{
  public MissingMatchingStrategyException()
  {
  }

  public MissingMatchingStrategyException(string? message) : base(message)
  {
  }

  public MissingMatchingStrategyException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
