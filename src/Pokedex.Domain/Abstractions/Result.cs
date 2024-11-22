using Pokedex.Domain.Exceptions;

namespace Pokedex.Domain.Abstractions;

public sealed class Result<T> where T : class
{
  private readonly T? _value;

  public Result(T? value, Error error)
  {
    ArgumentNullException.ThrowIfNull(error);

    if (value is null && error == Error.None)
    {
      throw NewInvalidResultObjectArgumentsException();
    }

    if (value is not null && error != Error.None)
    {
      throw NewInvalidResultObjectArgumentsException();
    }

    this.Error = error;
    this._value = value;

    static InvalidResultObjectArgumentsException NewInvalidResultObjectArgumentsException() =>
      new(
        $"The provided combination of {nameof(value)} and {nameof(error)} parameters is invalid"
      );
  }

  public Error Error { get; }
  public bool IsSuccess => Error == Error.None;
  public bool IsFailure => !IsSuccess;
  public T Value
  {
    get
    {
      if (this.IsSuccess)
      {
        return _value!;
      }

      throw new InvalidOperationException("Value of a failure result cannot be accessed.");
    }
  }

  public static implicit operator Result<T>(T value) => Result.Success(value);
  public static implicit operator Result<T>(Error error) => Result.Failure<T>(error);
}

public static class Result
{
  public static Result<T> Success<T>(T value) where T : class => new(value, Error.None);
  public static Result<T> Failure<T>(Error error) where T : class => new(default, error);
}
