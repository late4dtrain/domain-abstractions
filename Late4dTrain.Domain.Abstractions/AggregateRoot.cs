namespace Late4dTrain.Domain.Abstractions;

public abstract class AggregateRoot<TId, TDomainEvent> : Entity<TId>, IAggregateRoot<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public List<TDomainEvent> Events => new();
}
