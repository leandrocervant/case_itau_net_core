using CaseItau.Domain.Common.Interfaces;

namespace CaseItau.Domain.Common;

public class Entity
{
    public long Id { get; private set; }

    protected readonly List<IDomainEvent> _events = [];

    public List<IDomainEvent> PopEvents()
    {
        var copy = _events.ToList();

        _events.Clear();

        return copy;
    }
}
