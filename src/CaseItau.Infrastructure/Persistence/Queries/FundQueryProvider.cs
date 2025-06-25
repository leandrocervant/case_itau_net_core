using CaseItau.Application.Funds.Dto;
using CaseItau.Application.Funds.Queries;
using CaseItau.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Persistence.Queries
{
    internal class FundQueryProvider(FundsDbContext dbContext) : IFundQueryProvider
    {
        public Task<FundDto?> GetFundAsync(string code)
        {
            var fund = dbContext.Funds
                .AsNoTracking()
                .Where(f => f.Code == code)
                .Join(dbContext.FundTypes,
                    f => f.TypeId,
                    ft => ft.Id,
                    (fund, ft) => new { Fund = fund, FundType = ft })
                .Select(f => new FundDto(
                    f.Fund.Code,
                    f.Fund.Name,
                    f.Fund.Cnpj.Value,
                    f.Fund.TypeId,
                    f.FundType.Name,
                    f.Fund.Patrimony))
                .FirstOrDefaultAsync();

            return fund;
        }

        public Task<List<FundDto>> ListFundsAsync()
        {
            var funds = dbContext.Funds
                .AsNoTracking()
                .Join(dbContext.FundTypes,
                    f => f.TypeId,
                    ft => ft.Id,
                    (fund, ft) => new { Fund = fund, FundType = ft })
                .Select(f => new FundDto(
                    f.Fund.Code,
                    f.Fund.Name,
                    f.Fund.Cnpj.Value,
                    f.Fund.TypeId,
                    f.FundType.Name,
                    f.Fund.Patrimony))
                .ToListAsync();

            return funds;
        }
    }
}
