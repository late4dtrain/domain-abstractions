namespace Late4dTrain.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    public DateTimeOffset DateOccured => DateTimeOffset.UtcNow;
}
