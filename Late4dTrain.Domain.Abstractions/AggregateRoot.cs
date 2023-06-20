namespace Late4dTrain.Domain.Abstractions;

public abstract class AggregateRoot<TId, TDomainEvent> : Entity<TId>, IAggregateRoot<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    #pragma warning disable ConvertToAutoProperty
    private readonly List<TDomainEvent> _domainEvents = new();

    public List<TDomainEvent> Events => _domainEvents;
    #pragma warning restore ConvertToAutoProperty
}
