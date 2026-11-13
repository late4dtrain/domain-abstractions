namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// Base class for entities whose identity is assigned at construction and remains stable.
/// Equality is based on the exact runtime type and identity; other state is ignored.
/// </summary>
/// <typeparam name="TId">The identity type. Reference-type IDs should be immutable.</typeparam>
public abstract class Entity<TId> : IEntity<TId>, IEquatable<Entity<TId>>
{
    /// <summary>
    /// Initializes an entity with its identity.
    /// </summary>
    /// <param name="id">The identity. A null reference identity is not allowed; default value-type identities are permitted.</param>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is null.</exception>
    protected Entity(TId id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));

        Id = id;
    }

    /// <summary>
    /// Gets the immutable identity assigned at construction.
    /// </summary>
    public TId Id { get; }

    /// <inheritdoc />
    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return other.GetType() == GetType() && EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc />
    public sealed override bool Equals(object? obj) => obj is Entity<TId> other && Equals(other);

    /// <inheritdoc />
    public sealed override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id!);

    /// <summary>
    /// Compares two entities using their identity equality.
    /// </summary>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => object.Equals(left, right);

    /// <summary>
    /// Compares two entities using their identity equality.
    /// </summary>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !object.Equals(left, right);
}
