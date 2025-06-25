namespace CaseItau.Contracts.Funds;

public class UpdateFundRequest
{
    public string Name { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public long TypeId { get; set; }
}
