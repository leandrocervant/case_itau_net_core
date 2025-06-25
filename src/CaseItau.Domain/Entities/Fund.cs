using CaseItau.Domain.Common;
using CaseItau.Domain.Events.Fund;
using CaseItau.Domain.ValueObjects;

namespace CaseItau.Domain.Entities;

public class Fund : Entity, IAggregateRoot
{
    public Fund(string code, string name, Cnpj cnpj, long typeId)
    {
        Code = code;
        Name = name;
        Cnpj = cnpj;
        TypeId = typeId;

        Validate();

        _events.Add(new FundCreatedEvent(
            Id,
            code,
            DateTime.UtcNow));
    }

    private Fund() { }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Cnpj Cnpj { get; set; } = null!;

    public long TypeId { get; set; }

    public decimal Patrimony { get; set; }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
            throw new ArgumentException("Code cannot be null or empty.", nameof(Code));
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(Name));
        }

        if (Cnpj is null)
        {
            throw new ArgumentNullException(nameof(Cnpj), "CNPJ cannot be null.");
        }

        if (TypeId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(TypeId), "TypeId must be greater than zero.");
        }
    }
}
