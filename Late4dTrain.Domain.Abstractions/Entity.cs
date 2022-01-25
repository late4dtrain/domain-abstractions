namespace Late4dTrain.Domain.Abstractions;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Usage", "S3875", Justification = "Intentional override for Entity comparison")]
[SuppressMessage("Usage", "S4035", Justification = "Intentional override for Entity comparison")]
public abstract class Entity<TId> : IEntity<TId>
{
    public abstract TId Id { get; protected set; }

    protected bool Equals(Entity<TId> other) => EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;

        return other.GetType() == GetType() && Equals(other);
    }

    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id!);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !Equals(left, right);
}
