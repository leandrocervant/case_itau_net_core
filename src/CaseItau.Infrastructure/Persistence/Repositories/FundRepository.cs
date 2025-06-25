using CaseItau.Domain.Entities;
using CaseItau.Domain.Repositories;
using CaseItau.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Persistence.Repositories;

internal class FundRepository(FundsDbContext dbContext) : IFundRepository
{
    private readonly FundsDbContext _dbContext = dbContext;

    public async Task<bool> ExistsAsync(string code)
    {
        return await _dbContext.Funds.AnyAsync(f => f.Code == code);
    }

    public async Task<long> AddAsync(Fund fund)
    {
        var result = await _dbContext.Funds.AddAsync(fund);

        return result.Entity.Id;
    }

    public Task RemoveAsync(Fund fund)
    {
        _dbContext.Funds.Remove(fund);

        return Task.CompletedTask;
    }

    public async Task<List<Fund>> GetAllAsync()
    {
        var funds = await _dbContext.Funds.ToListAsync();

        return funds;
    }

    public Task<Fund?> GetByCodeAsync(string code)
    {
        var fund = _dbContext.Funds.FirstOrDefaultAsync(f => f.Code == code);

        return fund;
    }

    public async Task AdjustPatrimonyAsync(string code, decimal amount)
    {
        var affectedRows = await _dbContext.Funds
            .Where(
                f => f.Code == code)
            .ExecuteUpdateAsync(
                s => s.SetProperty(f => f.Patrimony, f => f.Patrimony + amount));

        if (affectedRows == 0)
        {
            throw new InvalidOperationException($"No fund found with code {code} to move patrimony of {amount}.");
        }
    }
}
