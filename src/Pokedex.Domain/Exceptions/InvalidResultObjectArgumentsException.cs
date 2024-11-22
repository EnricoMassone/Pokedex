namespace Pokedex.Domain.Exceptions;

public class InvalidResultObjectArgumentsException : Exception
{
  public InvalidResultObjectArgumentsException()
  {
  }

  public InvalidResultObjectArgumentsException(
    string? message) : base(message)
  {
  }

  public InvalidResultObjectArgumentsException(
    string? message,
    Exception? innerException) : base(message, innerException)
  {
  }
}
