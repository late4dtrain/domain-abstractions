namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// Infrastructure-facing access to an aggregate's pending domain events.
/// </summary>
public interface IDomainEventContainer
{
    /// <summary>
    /// Captures the pending events as an immutable snapshot without removing them.
    /// </summary>
    IReadOnlyList<IDomainEvent> CaptureDomainEvents();

    /// <summary>
    /// Acknowledges the captured event occurrences after the caller has processed them.
    /// Only matching event instances are removed; later events remain pending.
    /// </summary>
    /// <param name="capturedEvents">The events previously captured for processing.</param>
    void AcknowledgeDomainEvents(IEnumerable<IDomainEvent> capturedEvents);
}
