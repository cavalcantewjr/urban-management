using System.Text.RegularExpressions;

namespace UrbanManagement.Domain.ValueObjects;

public class ZipCode
{
    public string Value { get; set; }

    public ZipCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Zip code cannot be null or empty.", nameof(value));

        if (!Regex.IsMatch(value, @"^\d{5}-?\d{3}$"))
            throw new ArgumentException("Invalid zip code format.", nameof(value));
        Value = value;
    }

    public override bool Equals(object obj) =>
        obj is ZipCode other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

}
