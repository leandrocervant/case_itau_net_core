namespace CaseItau.Contracts.Funds;

public class CreateFundResponse
{
    public CreateFundResponse(string code)
    {
        Code = code;
    }

    public string Code { get; set; } = null!;
}
