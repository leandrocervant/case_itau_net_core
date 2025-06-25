using CaseItau.Domain.Entities;

namespace CaseItau.Domain.Repositories;

public interface IFundTypeRepository
{
    Task<long> AddAsync(FundType fundType);

    Task<List<FundType>> GetAll();

    Task<FundType?> GetByIdAsync(long id);
}
