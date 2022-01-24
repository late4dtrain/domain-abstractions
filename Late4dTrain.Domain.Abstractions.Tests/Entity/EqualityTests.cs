namespace Late4dTrain.Domain.Abstractions.Tests.Entity
{
    using System;

    using AutoFixture.Xunit2;

    using FluentAssertions;

    using Xunit;

    public class EqualityTests
    {
        [Theory]
        [AutoData]
        public void Entities_Without_Domain_Event_With_Same_Id_Will_Be_Equal(Guid id, string name1, string name2)
        {
            var first = new TestEntityWithoutDomainEvent(id, name1);
            var second = new TestEntityWithoutDomainEvent(id, name2);

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
            var first = new TestEntityWithoutDomainEvent(id1, name);
            var second = new TestEntityWithoutDomainEvent(id2, name);

            (first.Name == second.Name).Should().BeTrue();
            first.Equals(second).Should().BeFalse();
            (first == second).Should().BeFalse();
            (first != second).Should().BeTrue();
            first.GetHashCode().Equals(second.GetHashCode()).Should().BeFalse();
        }

        [Fact]
        public void Entities_Without_Domain_Event_Both_Null()
        {
            TestEntityWithoutDomainEvent? first = null;
            TestEntityWithoutDomainEvent? second = null;
            (first == second).Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Entities_With_Domain_Event_With_Same_Id_Will_Be_Equal(Guid id, string name1, string name2)
        {
            var first = new TestEntityWithDomainEvent(id, name1);
            var second = new TestEntityWithDomainEvent(id, name2);

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
            var first = new TestEntityWithDomainEvent(id1, name);
            var second = new TestEntityWithDomainEvent(id2, name);

            (first.Name == second.Name).Should().BeTrue();
            first.Equals(second).Should().BeFalse();
            (first == second).Should().BeFalse();
            (first != second).Should().BeTrue();
            first.GetHashCode().Equals(second.GetHashCode()).Should().BeFalse();
        }

        [Fact]
        public void Entities_With_Domain_Event_Both_Null()
        {
            TestEntityWithDomainEvent? first = null;
            TestEntityWithDomainEvent? second = null;
            (first == second).Should().BeTrue();
        }
    }
}
