namespace Late4dTrain.Domain.Abstractions.Tests.Entity;

using System;

using Abstractions;

internal class TestEntityWithDomainEvent
    : Entity<Guid>.WithDomainEvent<DomainEvent>, IAggregateRoot
{
    public TestEntityWithDomainEvent(Guid id, string name)
    {
        (Id, Name) = (id, name);
        Events.Add(new TestEntityCreated());
    }

    public string Name { get; }

    public sealed override Guid Id { get; protected set; }
}
