using CaseItau.Domain.Entities;

namespace CaseItau.UnitTests.Common.Builders;

public class FundTypeBuilder
{
    private string _name = "Default Fund Type";

    public FundTypeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FundType Build()
    {
        return new FundType(_name);
    }

    public static FundTypeBuilder New() => new();
}
