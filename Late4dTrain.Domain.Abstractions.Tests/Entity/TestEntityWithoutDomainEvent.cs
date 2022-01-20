namespace Late4dTrain.Domain.Abstractions.Tests.Entity;

using System;

using Abstractions;

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
