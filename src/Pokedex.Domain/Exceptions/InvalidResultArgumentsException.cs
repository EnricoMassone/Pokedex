namespace Pokedex.Domain.Exceptions;

public class InvalidResultArgumentsException : Exception
{
  public InvalidResultArgumentsException()
  {
  }

  public InvalidResultArgumentsException(
    string? message) : base(message)
  {
  }

  public InvalidResultArgumentsException(
    string? message,
    Exception? innerException) : base(message, innerException)
  {
  }
}
