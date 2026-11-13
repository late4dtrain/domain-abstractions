using System;
using System.Collections.Generic;

using AutoFixture.Xunit2;

using FluentAssertions;

using Xunit;

namespace Late4dTrain.Domain.Abstractions.Tests.ValueObject
{
    using Data;

    public class EqualityTests
    {
        [Theory]
        [AutoData]
        public void ValueObject_With_Same_Values_Will_Be_Equal(int first, int second, int third)
        {
            var valueObject1 = new TestValueObject(first, second, third, "first description");
            var valueObject2 = new TestValueObject(first, second, third, "different description");

            ReferenceEquals(valueObject1, valueObject2).Should().BeFalse();
            valueObject1.Equals(valueObject2).Should().BeTrue();
            (valueObject1 == valueObject2).Should().BeTrue();
            (valueObject1 != valueObject2).Should().BeFalse();
            valueObject1.GetHashCode().Should().Be(valueObject2.GetHashCode());
        }

        [Theory]
        [AutoData]
        public void ValueObject_With_Different_Values_Will_Be_UnEqual(
            int first1,
            int first2,
            int second,
            int third)
        {
            var valueObject1 = new TestValueObject(first1, second, third);
            var valueObject2 = new TestValueObject(first2, second, third);

            ReferenceEquals(valueObject1, valueObject2).Should().BeFalse();
            valueObject1.Equals(valueObject2).Should().BeFalse();
            (valueObject1 == valueObject2).Should().BeFalse();
            (valueObject1 != valueObject2).Should().BeTrue();
        }

        [Fact]
        public void ValueObject_Null_Equality()
        {
            TestValueObject? valueObject1 = null;
            TestValueObject? valueObject2 = null;
            (valueObject1 == valueObject2).Should().BeTrue();
            (valueObject1 != valueObject2).Should().BeFalse();
        }

        [Fact]
        public void ValueObject_Is_Equal_To_Itself()
        {
            var valueObject = new TestValueObject(1, 2, 3);
            var sameValueObject = valueObject;

            valueObject.Equals(valueObject).Should().BeTrue();
            (valueObject == sameValueObject).Should().BeTrue();
        }

        [Fact]
        public void ValueObject_Compared_With_Null_Is_Unequal_In_Both_Operand_Orders()
        {
            var valueObject = new TestValueObject(1, 2, 3);
            TestValueObject? nullValueObject = null;

            (valueObject == nullValueObject).Should().BeFalse();
            (nullValueObject == valueObject).Should().BeFalse();
            (valueObject != nullValueObject).Should().BeTrue();
            (nullValueObject != valueObject).Should().BeTrue();
        }

        [Fact]
        public void ValueObject_Is_Not_Equal_To_An_Unrelated_Object()
        {
            var valueObject = new TestValueObject(1, 2, 3);

            valueObject.Equals(new object()).Should().BeFalse();
        }

        [Fact]
        public void Different_ValueObject_Types_With_The_Same_Properties_Are_Unequal()
        {
            var first = new TestValueObject(1, 2, 3);
            var second = new DifferentTestValueObject(1, 2, 3);

            first.Equals(second).Should().BeFalse();
            second.Equals(first).Should().BeFalse();
        }

        [Fact]
        public void Equality_And_Hashing_Do_Not_Invoke_Unselected_Properties()
        {
            var first = new ValueObjectWithThrowingProperties(42);
            var second = new ValueObjectWithThrowingProperties(42);

            first.Equals(second).Should().BeTrue();
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void Null_Equality_Components_Are_Supported()
        {
            var first = new SingleComponentValueObject(null);
            var second = new SingleComponentValueObject(null);

            first.Equals(second).Should().BeTrue();
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void Empty_Equality_Components_Make_Instances_Of_The_Same_Type_Equal()
        {
            var first = new EmptyValueObject();
            var second = new EmptyValueObject();

            first.Equals(second).Should().BeTrue();
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void Collection_ValueObject_Uses_Ordered_Contents_And_Copies_Input()
        {
            var input = new[] { "alpha", "beta" };
            var first = new CollectionValueObject(input);
            var equivalent = new CollectionValueObject(new[] { "alpha", "beta" });
            var differentOrder = new CollectionValueObject(new[] { "beta", "alpha" });
            input[0] = "changed";

            first.Equals(equivalent).Should().BeTrue();
            first.GetHashCode().Should().Be(equivalent.GetHashCode());
            first.Equals(differentOrder).Should().BeFalse();
            first.Values.Should().Equal("alpha", "beta");
        }

        [Fact]
        public void Unequal_Components_Can_Have_Equal_Hash_Codes()
        {
            var first = new CollisionValueObject(new CollisionComponent(1));
            var second = new CollisionValueObject(new CollisionComponent(2));

            first.GetHashCode().Should().Be(second.GetHashCode());
            first.Equals(second).Should().BeFalse();
            new HashSet<Abstractions.ValueObject> { first, second }.Should().HaveCount(2);
        }

        private sealed class DifferentTestValueObject : Abstractions.ValueObject
        {
            public DifferentTestValueObject(int first, int second, int third) =>
                (First, Second, Third) = (first, second, third);

            public int First { get; }

            public int Second { get; }

            public int Third { get; }

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return First;
                yield return Second;
                yield return Third;
            }
        }

        private sealed class ValueObjectWithThrowingProperties : Abstractions.ValueObject
        {
            public ValueObjectWithThrowingProperties(int value) => Value = value;

            public int Value { get; }

            public int this[int index] => throw new InvalidOperationException("Indexer must not be inspected.");

            public int Computed => throw new InvalidOperationException("Computed property must not be inspected.");

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Value;
            }
        }

        private sealed class SingleComponentValueObject : Abstractions.ValueObject
        {
            public SingleComponentValueObject(object? value) => Value = value;

            public object? Value { get; }

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Value;
            }
        }

        private sealed class EmptyValueObject : Abstractions.ValueObject
        {
            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield break;
            }
        }

        private sealed class CollectionValueObject : Abstractions.ValueObject
        {
            private readonly string[] _values;

            public CollectionValueObject(IEnumerable<string> values) => _values = new List<string>(values).ToArray();

            public IReadOnlyList<string> Values => Array.AsReadOnly(_values);

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return _values.Length;
                foreach (var value in _values)
                {
                    yield return value;
                }
            }
        }

        private readonly struct CollisionComponent : IEquatable<CollisionComponent>
        {
            public CollisionComponent(int value) => Value = value;

            private int Value { get; }

            public bool Equals(CollisionComponent other) => Value == other.Value;

            public override bool Equals(object? obj) => obj is CollisionComponent other && Equals(other);

            public override int GetHashCode() => 1;
        }

        private sealed class CollisionValueObject : Abstractions.ValueObject
        {
            public CollisionValueObject(CollisionComponent component) => Component = component;

            private CollisionComponent Component { get; }

            protected override IEnumerable<object?> GetEqualityComponents()
            {
                yield return Component;
            }
        }
    }
}
