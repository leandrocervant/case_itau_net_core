namespace CaseItau.Contracts.Funds;

public class CreateFundRequest
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public long TypeId { get; set; }
}
