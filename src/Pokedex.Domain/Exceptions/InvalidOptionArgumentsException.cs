namespace Pokedex.Domain.Exceptions;

public class InvalidOptionArgumentsException : Exception
{
  public InvalidOptionArgumentsException()
  {
  }

  public InvalidOptionArgumentsException(string? message) : base(message)
  {
  }

  public InvalidOptionArgumentsException(string? message, Exception? innerException) : base(message, innerException)
  {
  }
}
