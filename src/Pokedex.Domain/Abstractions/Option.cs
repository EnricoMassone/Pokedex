using Pokedex.Domain.Exceptions;

namespace Pokedex.Domain.Abstractions;

public sealed class Option<T> where T : class
{
  private readonly T? _value;

  public Option(T? value, bool hasValue)
  {
    if (value is null && hasValue)
    {
      throw NewInvalidOptionArgumentsException();
    }

    if (value is not null && !hasValue)
    {
      throw NewInvalidOptionArgumentsException();
    }

    this.HasValue = hasValue;
    this._value = value;

    static InvalidOptionArgumentsException NewInvalidOptionArgumentsException() =>
      new(
        $"The provided combination of {nameof(value)} and {nameof(hasValue)} parameters is invalid"
      );
  }

  public bool HasValue { get; }
  public T Value
  {
    get
    {
      if (this.HasValue)
      {
        return _value!;
      }

      throw new InvalidOperationException("Value of an empty option cannot be accessed.");
    }
  }

  public static readonly Option<T> None = new(default, hasValue: false);

  public static implicit operator Option<T>(T value) => Option.Some(value);
}

public static class Option
{
  public static Option<T> Some<T>(T value) where T : class => new(value, hasValue: true);
}
