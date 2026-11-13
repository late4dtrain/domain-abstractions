namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// An entity with a typed, read-only identity.
/// </summary>
/// <typeparam name="TId">The identity type.</typeparam>
public interface IEntity<out TId>
{
    /// <summary>
    /// Gets the entity identity.
    /// </summary>
    public TId Id { get; }
}
