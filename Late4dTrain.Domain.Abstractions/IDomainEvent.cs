namespace Late4dTrain.Domain.Abstractions;

public interface IDomainEvent
{
    public DateTimeOffset DateOccurred { get; }
}
