namespace Late4dTrain.Domain.Abstractions;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Usage", "S3875", Justification = "Intentional override for Entity comparison")]
[SuppressMessage("Usage", "S4035", Justification = "Intentional override for Entity comparison")]
public abstract class AggregateRoot<TId, TDomainEvent> : Entity<TId>, IAggregateRoot where TDomainEvent : IDomainEvent
{
    public List<TDomainEvent> Events => new();
}
