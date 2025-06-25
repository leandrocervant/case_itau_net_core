using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Repositories;

public interface IFundRepository
{
    Task<bool> ExistsAsync(string code);

    Task<long> AddAsync(Fund fund);

    Task RemoveAsync(Fund fund);

    Task<List<Fund>> GetAllAsync();

    Task<Fund?> GetByCodeAsync(string code);

    Task AdjustPatrimonyAsync(string code, decimal amount);
}
