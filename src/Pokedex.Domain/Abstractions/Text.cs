namespace Pokedex.Domain.Abstractions;

public sealed record Text
{
  public string Value { get; }

  public Text(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("The value of a text cannot be null or white space", nameof(value));
    }

    this.Value = value;
  }

  public static implicit operator string(Text text) => text.Value;
}
