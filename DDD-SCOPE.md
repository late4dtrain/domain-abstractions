# Domain-driven design scope

Late4dTrain.Domain.Abstractions is a dependency-free tactical DDD building-block library. Its runtime package targets .NET Standard 2.0 and C# 10. Consumer tests target .NET 10.

The runtime library provides explicit contracts for entity identity, value equality, aggregate-root boundaries, and pending domain events. A separate analyzer package diagnoses selected structural conventions. Compiled consumer examples and tests demonstrate those contracts.

The consuming domain remains responsible for ubiquitous language, bounded contexts, aggregate boundaries, business invariants, factories, and deciding where domain services are appropriate. The library does not certify that a model is correct DDD. Persistence, repositories, specifications, messaging, event sourcing, outbox, ORM, and CQRS integrations are optional extensions; the core does not depend on them.
