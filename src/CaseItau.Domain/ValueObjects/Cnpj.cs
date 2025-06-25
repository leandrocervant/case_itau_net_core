using CaseItau.Domain.Common;

namespace CaseItau.Domain.ValueObjects;

public class Cnpj : ValueObject
{
    public Cnpj(string value)
    {
        Value = value;

        Validate();
    }

    public string Value { get; }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            throw new ArgumentException("CNPJ cannot be null or empty.", nameof(Value));
        }

        if (Value.Length != 14 || !long.TryParse(Value, out _))
        {
            throw new ArgumentException("CNPJ must be a 14-digit number.", nameof(Value));
        }
    }
}
