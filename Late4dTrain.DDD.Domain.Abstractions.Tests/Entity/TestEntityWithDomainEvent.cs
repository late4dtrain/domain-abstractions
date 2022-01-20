namespace Late4dTrain.DDD.Domain.Abstractions.Tests.Entity;

using System;

internal class TestEntityWithDomainEvent
    : Entity<Guid>.WithDomainEvent<DomainEvent>, IAggregateRoot
{
    public TestEntityWithDomainEvent(Guid id, string name)
    {
        (Id, Name) = (id, name);
        Events.Add(new TestEntityCreated());
    }

    public string Name { get; }
}
