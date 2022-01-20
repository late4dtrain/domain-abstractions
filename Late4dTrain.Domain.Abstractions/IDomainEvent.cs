namespace Late4dTrain.DDD.Domain.Abstractions;

public interface IDomainEvent
{
    public DateTimeOffset DateOccured { get; }
}
