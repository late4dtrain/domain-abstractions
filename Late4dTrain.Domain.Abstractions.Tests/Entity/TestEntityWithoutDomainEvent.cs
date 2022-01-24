using System;

namespace Late4dTrain.Domain.Abstractions.Tests.Entity
{
    internal class TestEntityWithoutDomainEvent
        : Entity<Guid>.WithoutDomainEvent
    {
        public TestEntityWithoutDomainEvent(Guid id, string name) => (Id, Name) = (id, name);

        public string Name { get; }

        public sealed override Guid Id { get; protected set; }
    }

    public class TestEntityCreated : DomainEvent
    {
        public string EventName => nameof(TestEntityCreated);
    }
}
