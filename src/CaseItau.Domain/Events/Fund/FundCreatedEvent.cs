using CaseItau.Domain.Common.Interfaces;

namespace CaseItau.Domain.Events.Fund;

public record FundCreatedEvent(
    long Id,
    string Code,
    DateTime CreatedAt
    ) : IDomainEvent;
