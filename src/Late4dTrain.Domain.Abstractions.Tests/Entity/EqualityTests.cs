namespace Late4dTrain.Domain.Abstractions.Tests.Entity
{
    using System;
    using System.Collections.Generic;

    using AutoFixture.Xunit2;

    using Data;

    using FluentAssertions;

    using Xunit;

    public class EqualityTests
    {
        [Theory]
        [AutoData]
        public void Entities_Without_Domain_Event_With_Same_Id_Will_Be_Equal(Guid id, string name1, string name2)
        {
            var first = new TestEntity(id, name1);
            var second = new TestEntity(id, name2);

            (first.Name != second.Name).Should().BeTrue();
            first.Equals(second).Should().BeTrue();
            (first == second).Should().BeTrue();
            (first != second).Should().BeFalse();
            first.GetHashCode().Equals(second.GetHashCode()).Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Entities_Without_Domain_Event_With_Different_Id_Will_Be_Unequal(Guid id1, Guid id2, string name)
        {
            var first = new TestEntity(id1, name);
            var second = new TestEntity(id2, name);

            (first.Name == second.Name).Should().BeTrue();
            first.Equals(second).Should().BeFalse();
            (first == second).Should().BeFalse();
            (first != second).Should().BeTrue();
        }

        [Fact]
        public void Entities_Without_Domain_Event_Both_Null()
        {
            TestEntity? first = null;
            TestEntity? second = null;
            (first == second).Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Entities_With_Domain_Event_With_Same_Id_Will_Be_Equal(Guid id, string name1, string name2)
        {
            var first = new TestAggregateRoot(id, name1);
            var second = new TestAggregateRoot(id, name2);

            (first.Name != second.Name).Should().BeTrue();
            first.Equals(second).Should().BeTrue();
            (first == second).Should().BeTrue();
            (first != second).Should().BeFalse();
            first.GetHashCode().Equals(second.GetHashCode()).Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Entities_With_Domain_Event_With_Different_Id_Will_Be_Unequal(Guid id1, Guid id2, string name)
        {
            var first = new TestAggregateRoot(id1, name);
            var second = new TestAggregateRoot(id2, name);

            (first.Name == second.Name).Should().BeTrue();
            first.Equals(second).Should().BeFalse();
            (first == second).Should().BeFalse();
            (first != second).Should().BeTrue();
        }

        [Fact]
        public void Entities_With_Domain_Event_Both_Null()
        {
            TestAggregateRoot? first = null;
            TestAggregateRoot? second = null;
            (first == second).Should().BeTrue();
        }

        [Fact]
        public void Entity_Is_Equal_To_Itself()
        {
            var entity = new TestEntity(Guid.NewGuid(), "customer");
            var sameEntity = entity;

            entity.Equals(entity).Should().BeTrue();
            (entity == sameEntity).Should().BeTrue();
            ((IEquatable<Abstractions.Entity<Guid>>)entity).Equals(sameEntity).Should().BeTrue();
        }

        [Fact]
        public void Entity_Compared_With_Null_Is_Unequal_In_Both_Operand_Orders()
        {
            var entity = new TestEntity(Guid.NewGuid(), "customer");
            TestEntity? nullEntity = null;

            (entity == nullEntity).Should().BeFalse();
            (nullEntity == entity).Should().BeFalse();
            (entity != nullEntity).Should().BeTrue();
            (nullEntity != entity).Should().BeTrue();
        }

        [Fact]
        public void Entity_Is_Not_Equal_To_An_Unrelated_Object()
        {
            var entity = new TestEntity(Guid.NewGuid(), "customer");

            entity.Equals(new object()).Should().BeFalse();
        }

        [Fact]
        public void Different_Entity_Types_With_The_Same_Id_Are_Unequal()
        {
            var id = Guid.NewGuid();
            var first = new TestEntity(id, "customer");
            var second = new DifferentTestEntity(id);

            first.Equals(second).Should().BeFalse();
            second.Equals(first).Should().BeFalse();
        }

        [Fact]
        public void Hash_Collisions_Do_Not_Make_Different_Entities_Equal_Or_Unfindable()
        {
            var first = new CollisionEntity(new CollisionId(1));
            var second = new CollisionEntity(new CollisionId(2));
            var entities = new HashSet<CollisionEntity> { first, second };
            var values = new Dictionary<CollisionEntity, string>
            {
                [first] = "first",
                [second] = "second"
            };

            first.GetHashCode().Should().Be(second.GetHashCode());
            first.Equals(second).Should().BeFalse();
            entities.Should().HaveCount(2);
            entities.Should().Contain(first);
            entities.Should().Contain(second);
            values.Should().HaveCount(2);
            values[first].Should().Be("first");
            values[second].Should().Be("second");
        }

        [Fact]
        public void Entity_Id_Is_Get_Only()
        {
            typeof(Abstractions.Entity<Guid>).GetProperty(nameof(Abstractions.Entity<Guid>.Id))!
                .SetMethod.Should().BeNull();
        }

        [Fact]
        public void Explicitly_Assigned_Default_ValueType_Id_Is_Allowed()
        {
            var first = new TestEntity(Guid.Empty, "first");
            var second = new TestEntity(Guid.Empty, "second");

            first.Equals(second).Should().BeTrue();
        }

        [Fact]
        public void Null_ReferenceType_Id_Is_Rejected()
        {
            FluentActions.Invoking(() => new StringEntity(null!))
                .Should().Throw<ArgumentNullException>()
                .WithParameterName("id");
        }

        [Fact]
        public void Changing_NonIdentity_State_Does_Not_Change_HashCollection_Lookup()
        {
            var entity = new MutableNameEntity(Guid.NewGuid(), "before");
            var entities = new HashSet<MutableNameEntity> { entity };
            var values = new Dictionary<MutableNameEntity, string> { [entity] = "stored" };

            entity.Name = "after";

            entities.Should().Contain(entity);
            values[entity].Should().Be("stored");
        }

        [Fact]
        public void Strongly_Typed_Ids_Remain_Distinct_And_Can_Validate_Their_Own_Rules()
        {
            var value = Guid.NewGuid();
            var order = new OrderEntity(new OrderId(value));
            var customer = new CustomerEntity(new CustomerId(value));

            order.Id.Value.Should().Be(customer.Id.Value);
            order.Equals(customer).Should().BeFalse();
            FluentActions.Invoking(() => new OrderId(Guid.Empty))
                .Should().Throw<ArgumentException>();
        }

        private sealed class DifferentTestEntity : Abstractions.Entity<Guid>
        {
            public DifferentTestEntity(Guid id) : base(id)
            {
            }
        }

        private readonly struct CollisionId : IEquatable<CollisionId>
        {
            public CollisionId(int value) => Value = value;

            private int Value { get; }

            public bool Equals(CollisionId other) => Value == other.Value;

            public override bool Equals(object? obj) => obj is CollisionId other && Equals(other);

            public override int GetHashCode() => 1;
        }

        private sealed class CollisionEntity : Abstractions.Entity<CollisionId>
        {
            public CollisionEntity(CollisionId id) : base(id)
            {
            }
        }

        private sealed class StringEntity : Abstractions.Entity<string>
        {
            public StringEntity(string id) : base(id)
            {
            }
        }

        private sealed class MutableNameEntity : Abstractions.Entity<Guid>
        {
            public MutableNameEntity(Guid id, string name) : base(id) => Name = name;

            public string Name { get; set; }
        }

        private readonly record struct OrderId
        {
            public OrderId(Guid value)
            {
                if (value == Guid.Empty) throw new ArgumentException("An order ID cannot be empty.", nameof(value));
                Value = value;
            }

            public Guid Value { get; }
        }

        private readonly record struct CustomerId(Guid Value);

        private sealed class OrderEntity : Abstractions.Entity<OrderId>
        {
            public OrderEntity(OrderId id) : base(id)
            {
            }
        }

        private sealed class CustomerEntity : Abstractions.Entity<CustomerId>
        {
            public CustomerEntity(CustomerId id) : base(id)
            {
            }
        }
    }
}
