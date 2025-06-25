using CaseItau.Domain.Common;

namespace CaseItau.Domain.Entities;

public class FundType : Entity, IAggregateRoot
{
    public string Name { get; private set; } = null!;

    public FundType(string name)
    {
        Name = name;
    }

    private FundType() { }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
