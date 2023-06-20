# Late4dTrain.Domain.Abstractions

[![.NET](https://github.com/late4dtrain/domain-abstractions/actions/workflows/ci.yml/badge.svg)](https://github.com/late4dtrain/domain-abstractions/actions/workflows/ci.yml)
[![Release to Nuget](https://github.com/late4dtrain/domain-abstractions/actions/workflows/release.yml/badge.svg)](https://github.com/late4dtrain/domain-abstractions/actions/workflows/release.yml)
[![NuGet version (GSoulavy.Csv.Net)](https://img.shields.io/nuget/v/Late4dTrain.Domain.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/Late4dTrain.Domain.Abstractions/) 

# Late4dTrain.Domain.Abstractions

This library provides a set of abstractions for Domain-Driven Design elements such as Entities, Value Objects, and Aggregate Roots. These abstractions serve as the building blocks for your domain model and enforce key characteristics required by each element.

## Installation

Install the library using the following command:
```bash
dotnet add package Late4dTrain.Domain.Abstractions
```

## Usage
Here is an example of how you can use the library:
```cs

public class Customer : AggregateRoot<Guid, DomainEvent>
{
    public string Name { get; private set; }

    public Customer(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Events.Add(new CustomerCreated(Id));
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
In this example, `Customer` is an Aggregate Root that uses a `Guid` as an identifier and `DomainEvent` as its domain event type. When a new customer is created, a `CustomerCreated` is added to the list of domain events.

### Entity

Entities are fundamental building blocks of domain-driven design. They are mutable objects with a consistent thread of identity. Here is a basic example of how to create an entity:

```csharp
public class SampleEntity : Entity<Guid>
{
    public override Guid Id { get; protected set; } = Guid.NewGuid();

    // Additional properties and methods here...
}
```
In this example, `SampleEntity` is an entity with a unique identifier of type `Guid`

### Value Object

Value objects are objects that we care about only because of their properties and the values those properties have. They don't have an identity, meaning two value objects with the same properties are considered equal. Here's an example:

```cs

public class SampleValueObject : IValueObject
{
    // Properties and methods here...
}

```
In this example, SampleValueObject is a value object.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
This project is licensed under the terms of the MIT license.
