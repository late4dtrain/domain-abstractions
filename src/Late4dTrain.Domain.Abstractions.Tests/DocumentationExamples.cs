namespace Late4dTrain.Domain.Abstractions.Tests;

using System;

using Late4dTrain.Domain.Abstractions;

using ValueObjectBase = Late4dTrain.Domain.Abstractions.ValueObject;

// These examples mirror the public README usage snippets so documentation changes
// are compiled as part of the test project.
internal sealed class ReadmeCustomer : AggregateRoot<Guid>
{
    public ReadmeCustomer(string name) : base(Guid.NewGuid())
    {
        Name = name;
        RaiseDomainEvent(new ReadmeCustomerCreated(Id));
    }

    public string Name { get; }
}

internal sealed class ReadmeCustomerCreated : DomainEvent
{
    public ReadmeCustomerCreated(Guid customerId) => CustomerId = customerId;

    public Guid CustomerId { get; }
}

internal sealed class ReadmeSampleEntity : Entity<Guid>
{
    public ReadmeSampleEntity(Guid id) : base(id)
    {
    }
}

internal sealed class ReadmeSampleValueObject : ValueObjectBase
{
    public ReadmeSampleValueObject(string value) => Value = value;

    public string Value { get; }

    protected override System.Collections.Generic.IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
