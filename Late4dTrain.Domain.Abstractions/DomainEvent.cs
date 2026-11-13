namespace Late4dTrain.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    /// <summary>
    /// Creates an event with the current UTC time as its occurrence instant.
    /// </summary>
    protected DomainEvent() : this(DateTimeOffset.UtcNow)
    {
    }

    /// <summary>
    /// Creates an event with an explicit occurrence instant.
    /// </summary>
    /// <param name="dateOccurred">The instant at which the event occurred.</param>
    protected DomainEvent(DateTimeOffset dateOccurred) => DateOccurred = dateOccurred.ToUniversalTime();

    /// <inheritdoc />
    public DateTimeOffset DateOccurred { get; }
}
