using CaseItau.Domain.Common;
using CaseItau.Domain.Common.Resouces;

namespace CaseItau.Domain.ValueObjects;

public class Cnpj : ValueObject, IEquatable<Cnpj>
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
            throw new DomainException(Errors.Cnpj_ValueCannotBeNullOrEmpty);
        }

        if (Value.Length != 14 || !long.TryParse(Value, out _))
        {
            throw new DomainException(Errors.Cnpj_ValueMustBe14Digit);
        }
    }

    public bool Equals(Cnpj? other) => Equals((object?)other);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null || GetType() != obj.GetType()) return false;
        var other = (Cnpj)obj;
        return Value == other.Value &&
               string.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Value.GetHashCode();
            hash = hash * 23 + (Value != null ? Value.GetHashCode() : 0);
            return hash;
        }
    }
}
