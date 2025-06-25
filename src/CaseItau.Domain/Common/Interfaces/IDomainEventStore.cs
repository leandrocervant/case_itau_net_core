namespace CaseItau.Domain.Common.Interfaces;

public interface IDomainEventStore
{
    Task<IEnumerable<IDomainEvent>> GetDomainEventsAsync();
}
