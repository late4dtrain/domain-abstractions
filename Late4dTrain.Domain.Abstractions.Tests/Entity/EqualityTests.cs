namespace Late4dTrain.Domain.Abstractions.Tests.Entity
{
    using System;

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
            first.GetHashCode().Equals(second.GetHashCode()).Should().BeFalse();
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
            first.GetHashCode().Equals(second.GetHashCode()).Should().BeFalse();
        }

        [Fact]
        public void Entities_With_Domain_Event_Both_Null()
        {
            TestAggregateRoot? first = null;
            TestAggregateRoot? second = null;
            (first == second).Should().BeTrue();
        }
    }
}
