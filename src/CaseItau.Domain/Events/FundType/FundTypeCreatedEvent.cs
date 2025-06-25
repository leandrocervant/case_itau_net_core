using CaseItau.Domain.Common.Interfaces;

namespace CaseItau.Domain.Events.FundType;

public record FundTypeCreatedEvent(
    long Id,
    DateTime CreatedAt
    ) : IDomainEvent;
