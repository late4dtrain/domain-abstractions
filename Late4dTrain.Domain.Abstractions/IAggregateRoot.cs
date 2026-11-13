namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// Marks an entity as an aggregate root.
/// </summary>
public interface IAggregateRoot
{
}

/// <summary>
/// An aggregate root with a strongly typed identity.
/// </summary>
/// <typeparam name="TId">The identity type.</typeparam>
public interface IAggregateRoot<out TId> : IEntity<TId>, IAggregateRoot
{
}
