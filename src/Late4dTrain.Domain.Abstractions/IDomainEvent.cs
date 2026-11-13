namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// A fact that has occurred in the domain.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the UTC instant when the event occurred.
    /// </summary>
    public DateTimeOffset DateOccurred { get; }
}
