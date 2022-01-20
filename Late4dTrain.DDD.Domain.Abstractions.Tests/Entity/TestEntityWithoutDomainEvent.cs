namespace Late4dTrain.DDD.Domain.Abstractions.Tests.Entity;

using System;

internal class TestEntityWithoutDomainEvent
    : Entity<Guid>.WithoutDomainEvent
{
    public TestEntityWithoutDomainEvent(Guid id, string name) => (Id, Name) = (id, name);

    public string Name { get; }
}

public class TestEntityCreated : DomainEvent
{
    public string EventName => nameof(TestEntityCreated);
}
