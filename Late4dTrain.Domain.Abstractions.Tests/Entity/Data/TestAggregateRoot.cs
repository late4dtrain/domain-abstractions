namespace Late4dTrain.Domain.Abstractions.Tests.Entity.Data;

using System;

internal class TestAggregateRoot
    : AggregateRoot<Guid, DomainEvent>
{
    public TestAggregateRoot(Guid id, string name)
    {
        (Id, Name) = (id, name);
        Events.Add(new TestAggregateCreated());
    }

    public string Name { get; }

    public sealed override Guid Id { get; protected set; }
}
