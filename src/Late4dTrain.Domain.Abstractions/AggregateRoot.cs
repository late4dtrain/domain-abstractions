namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// Base class for aggregate roots that record pending domain events.
/// </summary>
/// <typeparam name="TId">The aggregate identity type.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>, IDomainEventContainer
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Initializes the aggregate root with its stable identity.
    /// </summary>
    /// <param name="id">The aggregate identity.</param>
    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Records a domain event raised by this aggregate.
    /// </summary>
    /// <param name="domainEvent">The event that occurred.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null) throw new ArgumentNullException(nameof(domainEvent));

        _domainEvents.Add(domainEvent);
    }

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> CaptureDomainEvents() =>
        Array.AsReadOnly(_domainEvents.ToArray());

    /// <inheritdoc />
    public void AcknowledgeDomainEvents(IEnumerable<IDomainEvent> capturedEvents)
    {
        if (capturedEvents is null) throw new ArgumentNullException(nameof(capturedEvents));

        foreach (var capturedEvent in capturedEvents)
        {
            if (capturedEvent is null) throw new ArgumentException("Captured events cannot contain null.", nameof(capturedEvents));

            var index = _domainEvents.FindIndex(pendingEvent => ReferenceEquals(pendingEvent, capturedEvent));
            if (index >= 0) _domainEvents.RemoveAt(index);
        }
    }
}
