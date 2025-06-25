namespace CaseItau.Contracts.Funds;

public class CreateFundRequest
{
    public CreateFundRequest(string code, string name, string cnpj, long typeId)
    {
        Code = code;
        Name = name;
        Cnpj = cnpj;
        TypeId = typeId;
    }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public long TypeId { get; set; }
}
