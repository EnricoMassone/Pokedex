namespace Pokedex.Domain.Pokemons;

public sealed record Name
{
  public string Value { get; }

  public Name(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("The value of a name cannot be null or white space", nameof(value));
    }

    this.Value = value;
  }

  public static implicit operator string(Name name) => name.Value;
}
