namespace Late4dTrain.Domain.Abstractions;

using System.Diagnostics.CodeAnalysis;

public static class Entity<TId>
{
    [SuppressMessage("Usage", "S3875", Justification = "Intentional override for Entity comparison")]
    [SuppressMessage("Usage", "S4035", Justification = "Intentional override for Entity comparison")]
    public abstract class WithDomainEvent<TDomainEvent> : IEntity<TId> where TDomainEvent : IDomainEvent
    {
        public List<TDomainEvent> Events => new();

        public abstract TId Id { get; protected set; }

        protected bool Equals(WithDomainEvent<TDomainEvent> other) =>
            EqualityComparer<TId>.Default.Equals(Id, other.Id);

        public override bool Equals(object? obj)
        {
            if (obj is not WithDomainEvent<TDomainEvent> other) return false;
            if (ReferenceEquals(this, other)) return true;

            return other.GetType() == GetType() && Equals(other);
        }

        public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id!);

        public static bool operator ==(WithDomainEvent<TDomainEvent>? left, WithDomainEvent<TDomainEvent>? right) =>
            Equals(left, right);

        public static bool operator !=(WithDomainEvent<TDomainEvent>? left, WithDomainEvent<TDomainEvent>? right) =>
            !Equals(left, right);
    }

    [SuppressMessage("Usage", "S3875", Justification = "Intentional override for Entity comparison")]
    [SuppressMessage("Usage", "S4035", Justification = "Intentional override for Entity comparison")]
    public abstract class WithoutDomainEvent : IEntity<TId>
    {
        public abstract TId Id { get; protected set; }

        protected bool Equals(WithoutDomainEvent other) => EqualityComparer<TId>.Default.Equals(Id, other.Id);

        public override bool Equals(object? obj)
        {
            if (obj is not WithoutDomainEvent other) return false;
            if (ReferenceEquals(this, other)) return true;

            return other.GetType() == GetType() && Equals(other);
        }

        public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id!);

        public static bool operator ==(WithoutDomainEvent? left, WithoutDomainEvent? right) => Equals(left, right);

        public static bool operator !=(WithoutDomainEvent? left, WithoutDomainEvent? right) => !Equals(left, right);
    }
}
