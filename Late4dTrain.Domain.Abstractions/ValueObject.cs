namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// Base class for immutable value objects with equality defined by explicitly selected components.
/// </summary>
public abstract class ValueObject : IValueObject, IEquatable<ValueObject>
{
    /// <summary>
    /// Gets the immutable values that determine equality, in a stable order.
    /// Components should themselves be immutable. If a sequence is part of the value,
    /// enumerate its contents explicitly rather than yielding a mutable collection.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return other.GetType() == GetType() && EqualityComponents.SequenceEqual(other.EqualityComponents);
    }

    /// <summary>
    /// Determines whether this value object is equal to another object.
    /// </summary>
    public sealed override bool Equals(object? obj) => obj is ValueObject other && Equals(other);

    /// <summary>
    /// Gets a hash code from the same ordered components used for equality.
    /// </summary>
    public sealed override int GetHashCode()
    {
        unchecked
        {
            var hashCode = 17;
            foreach (var component in EqualityComponents)
            {
                hashCode = hashCode * 23 + (component?.GetHashCode() ?? 0);
            }

            return hashCode;
        }
    }

    private IEnumerable<object?> EqualityComponents =>
        GetEqualityComponents() ?? throw new InvalidOperationException("Equality components cannot be null.");

    /// <summary>
    /// Compares two value objects using their value equality.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right) => object.Equals(left, right);

    /// <summary>
    /// Compares two value objects using their value equality.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right) => !object.Equals(left, right);
}
