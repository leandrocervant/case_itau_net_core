using CaseItau.Domain.Entities;
using CaseItau.Domain.ValueObjects;

namespace CaseItau.UnitTests.Common.Builders;

public class FundBuilder
{
    private string _code = "DEFAULT_CODE";
    private string _name = "Default Fund Name";
    private string _cnpj = "12345678000195";
    private long _typeId = 1;
    private decimal _patrimony = 0;

    public FundBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public FundBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public FundBuilder WithCnpj(string cnpj)
    {
        _cnpj = cnpj;
        return this;
    }

    public FundBuilder WithTypeId(long typeId)
    {
        _typeId = typeId;
        return this;
    }

    public FundBuilder WithPatrimony(decimal patrimony)
    {
        _patrimony = patrimony;
        return this;
    }

    public Fund Build()
    {
        var fund = new Fund(_code, _name, new Cnpj(_cnpj), _typeId);
        fund.Patrimony = _patrimony;
        return fund;
    }

    public static FundBuilder New() => new();
}
