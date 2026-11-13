# Late4dTrain.Domain.Abstractions

[![.NET](https://github.com/late4dtrain/domain-abstractions/actions/workflows/ci.yml/badge.svg)](https://github.com/late4dtrain/domain-abstractions/actions/workflows/ci.yml)
[![Release to Nuget](https://github.com/late4dtrain/domain-abstractions/actions/workflows/release.yml/badge.svg)](https://github.com/late4dtrain/domain-abstractions/actions/workflows/release.yml)
[![NuGet version (GSoulavy.Csv.Net)](https://img.shields.io/nuget/v/Late4dTrain.Domain.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/Late4dTrain.Domain.Abstractions/) 

# Late4dTrain.Domain.Abstractions

This library provides a set of abstractions for Domain-Driven Design elements such as Entities, Value Objects, and Aggregate Roots. These abstractions serve as the building blocks for your domain model and enforce key characteristics required by each element.

See the [guarantee matrix](GUARANTEES.md) for a clear separation between runtime contracts, analyzer warnings, consumer responsibilities, and non-guarantees.

## Installation

Install the library using the following command:
```bash
dotnet add package Late4dTrain.Domain.Abstractions
```

## Usage
Here is an example of how you can use the library:
```cs
using System;
using System.Collections.Generic;
using Late4dTrain.Domain.Abstractions;

public class Customer : AggregateRoot<Guid>
{
    public string Name { get; }

    public Customer(string name) : base(Guid.NewGuid())
    {
        Name = name;
        RaiseDomainEvent(new CustomerCreated(Id));
    }
}

public class CustomerCreated : DomainEvent
{
    public Guid CustomerId { get; }

    public CustomerCreated(Guid customerId)
    {
        CustomerId = customerId;
    }
}

```

Aggregates record events through protected `RaiseDomainEvent`. Infrastructure can call `CaptureDomainEvents()` to obtain an immutable snapshot, then call `AcknowledgeDomainEvents(snapshot)` only after processing succeeds. Capturing alone does not clear pending events, and events raised after a snapshot are preserved when that snapshot is acknowledged. This in-memory lifecycle does not provide durable delivery or transaction guarantees.

`DomainEvent` captures its occurrence time once. Derive events can use the protected timestamp constructor when replaying or testing historical facts; supplied offsets are normalized to UTC. Dispatch is an application/infrastructure concern: coordinate event publication with persistence (often through an outbox) when consistency matters. Domain events are not automatically integration messages or event-sourcing records.
In this example, `Customer` is an aggregate root with a `Guid` identity. Creating it records a `CustomerCreated` fact in its pending events.

The test project contains a compiled [reference `Order` domain](src/Late4dTrain.Domain.Abstractions.Tests/ReferenceDomain/Order.cs) demonstrating typed identities, immutable `Money`, owned line items, and invariant-checked operations. It is illustrative, not a required modeling template.

### Optional analyzers

Consumers can install `Late4dTrain.Domain.Abstractions.Analyzers` for opt-in warnings on writable value-object/event state, mutable entity identity, mutable aggregate collection exposure, and marker-only value objects without equality. Rules are warnings by default; see the [analyzer guide](src/Late4dTrain.Domain.Abstractions.Analyzers/README.md) for diagnostic IDs and a strict `.editorconfig` profile. The runtime package does not depend on Roslyn.

### Optional repository port

`IAggregateRepository<TAggregate, TId>` constrains persistence ports to aggregate roots and defines lookup (`null` means missing), insert, update, and cancellation-token contracts. Implementations determine the storage commit strategy; the methods do not imply a cross-aggregate unit of work or atomic event publication. Keep query/read-model interfaces separate and return domain-specific projections rather than `IQueryable`.

### Optional specifications

Implement `ISpecification<T>` for a pure business predicate and compose it with `And`, `Or`, or `Not`. Composition short-circuits and rejects null specification operands; candidate null behavior is left to each domain predicate. Specifications are not persistence query expressions and are not required for ordinary validation.

### Entity

Entities are fundamental building blocks of domain-driven design. They are mutable objects with a consistent thread of identity. Here is a basic example of how to create an entity:

```csharp
public class SampleEntity : Entity<Guid>
{
    public SampleEntity(Guid id) : base(id)
    {
    }

    // Additional properties and methods here...
}
```
In this example, `SampleEntity` is an entity with a unique identifier of type `Guid`

### Value Object

Value objects are objects that we care about only because of their properties and the values those properties have. They don't have an identity, meaning two value objects with the same properties are considered equal. Here's an example:

```cs

public class SampleValueObject : ValueObject
{
    public SampleValueObject(string value) => Value = value;

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

```
In this example, `SampleValueObject` inherits value equality from `ValueObject`. A derived class explicitly selects the ordered components that determine equality by overriding `GetEqualityComponents`; unselected properties do not participate. Keep the selected components immutable, especially when instances are used as hash-collection keys.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
This project is licensed under the terms of the MIT license.
