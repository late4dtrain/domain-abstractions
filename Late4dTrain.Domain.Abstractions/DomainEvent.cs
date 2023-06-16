namespace Late4dTrain.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    #pragma warning disable ConvertToAutoProperty
    private readonly DateTimeOffset _dateOccured = DateTimeOffset.UtcNow;

    public DateTimeOffset DateOccured => _dateOccured;
    #pragma warning restore ConvertToAutoProperty
}
