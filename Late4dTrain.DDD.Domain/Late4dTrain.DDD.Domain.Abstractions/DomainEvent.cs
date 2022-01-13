namespace Late4dTrain.DDD.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    public DateTimeOffset DateOccured => DateTimeOffset.UtcNow;
}
