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
}
