namespace CaseItau.Contracts.Funds;

public class FundResponse
{
    public FundResponse(string code, string name, string cnpj, long typeId, string typeName, decimal patrimony)
    {
        Code = code;
        Name = name;
        Cnpj = cnpj;
        TypeId = typeId;
        TypeName = typeName;
        Patrimony = patrimony;
    }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public long TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public decimal Patrimony { get; set; }
}
