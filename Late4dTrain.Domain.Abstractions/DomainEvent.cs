namespace Late4dTrain.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    #pragma warning disable ConvertToAutoProperty
    private readonly DateTimeOffset _dateOccurred = DateTimeOffset.UtcNow;

    public DateTimeOffset DateOccurred => _dateOccurred;
    #pragma warning restore ConvertToAutoProperty
}
