using CaseItau.Domain.Entities;
using CaseItau.Domain.Repositories;
using CaseItau.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Persistence.Repositories;

internal class FundTypeRepository(FundsDbContext dbContext) : IFundTypeRepository
{
    private readonly FundsDbContext _dbContext = dbContext;

    public async Task<long> AddAsync(FundType fundType)
    {
        var result = await _dbContext.FundTypes.AddAsync(fundType);

        return result.Entity.Id;
    }

    public async Task<List<FundType>> GetAll()
    {
        var fundTypes = await _dbContext.FundTypes.ToListAsync();

        return fundTypes;
    }

    public Task<FundType?> GetByIdAsync(long id)
    {
        var fundType = _dbContext.FundTypes.FirstOrDefaultAsync(ft => ft.Id == id);

        return fundType;
    }
}
