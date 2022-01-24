﻿// Based on vkhorikov ValueObject type
// https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/c66b80ba70/CSharpFunctionalExtensions/ValueObject/ValueObject.cs

namespace Late4dTrain.Domain.Abstractions;

using System.Diagnostics.CodeAnalysis;

public static class ValueObject
{
    public abstract record AsRecord : IValueObject
    {
    }

    [SuppressMessage("Usage", "S3875", Justification = "Intentional override for Entity comparison")]
    [SuppressMessage("Usage", "S4035", Justification = "Intentional override for Entity comparison")]
    public abstract class AsClass : IValueObject
    {
        private int? _cachedHashCode;

        private int CachedHashCode
        {
            get
            {
                _cachedHashCode ??= GetPropertyValues()
                    .Aggregate(
                        1,
                        (current, obj) =>
                        {
                            unchecked
                            {
                                return current * 23 + (obj?.GetHashCode() ?? 0);
                            }
                        }
                    );

                return _cachedHashCode.Value;
            }
        }

        protected bool Equals(AsClass other) => GetPropertyValues().SequenceEqual(other.GetPropertyValues());

        private IEnumerable<object?> GetPropertyValues() =>
            GetType()
                .GetProperties()
                .Select(
                    propertyInfo => propertyInfo
                        .GetValue(this)
                );

        public override bool Equals(object? obj)
        {
            if (obj is not AsClass other) return false;
            if (ReferenceEquals(this, other)) return true;

            return other.GetType() == GetType() && Equals(other);
        }

        public override int GetHashCode() => CachedHashCode;

        public static bool operator ==(AsClass? left, AsClass? right) => Equals(left, right);

        public static bool operator !=(AsClass? left, AsClass? right) => !Equals(left, right);
    }
}
