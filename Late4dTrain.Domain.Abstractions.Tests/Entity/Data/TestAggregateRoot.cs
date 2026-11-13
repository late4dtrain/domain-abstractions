namespace Late4dTrain.Domain.Abstractions.Tests.Entity.Data;

using System;

internal class TestAggregateRoot
    : AggregateRoot<Guid>
{
    public TestAggregateRoot(Guid id, string name) : base(id)
    {
        Name = name;
        RaiseDomainEvent(new TestAggregateCreated());
    }

    public string Name { get; }
}
