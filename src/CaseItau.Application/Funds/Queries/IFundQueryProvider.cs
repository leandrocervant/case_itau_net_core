using CaseItau.Application.Funds.Dto;

namespace CaseItau.Application.Funds.Queries
{
    public interface IFundQueryProvider
    {
        Task<FundDto?> GetFundAsync(string code);

        Task<List<FundDto>> ListFundsAsync();
    }
}
