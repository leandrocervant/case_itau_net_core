namespace CaseItau.Domain.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}
