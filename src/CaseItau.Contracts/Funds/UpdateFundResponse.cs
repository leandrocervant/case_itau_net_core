namespace CaseItau.Contracts.Funds;

public class UpdateFundResponse
{
    public string Name { get; init; }

    public string Cnpj { get; init; }

    public long TypeId { get; init; }

    public UpdateFundResponse(string name, string cnpj, long typeId)
    {
        Name = name;
        Cnpj = cnpj;
        TypeId = typeId;
    }
}
