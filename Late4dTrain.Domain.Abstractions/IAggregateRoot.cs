namespace Late4dTrain.Domain.Abstractions;

public interface IAggregateRoot<TDomainEvent> where TDomainEvent : IDomainEvent
{
    List<TDomainEvent> Events { get; }
}
