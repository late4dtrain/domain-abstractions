# 00 — DDD review and roadmap

Status: approved and implemented. Reviewed against HEAD `547ff3c47986542b1a5ca30a93545b90e73495fa` and the initialization work. The owner confirms the packages are unreleased: breaking changes are acceptable and no migration guide is needed. Required increments 01–07 and release-readiness increment 10 are complete; optional repository and specification increments 08–09 were also implemented.

## Findings

- `Late4dTrain.Domain.Abstractions/ValueObject.cs:18-42`: cached hashes can disagree with equality when properties change. Reflection also includes static/computed properties, can throw for indexers, and compares collections according to their own equality rather than by elements.
- `Late4dTrain.Domain.Abstractions/AggregateRoot.cs:7-9`: the public mutable event list permits callers to add, remove, reorder, or clear events outside domain operations.
- `Late4dTrain.Domain.Abstractions/Entity.cs:9-21`: identity can change, invalidating hash-collection membership; two same-type entities with default IDs compare equal, even if consumers mean “unassigned.” Default values are not universally invalid, so the policy must be explicit.
- `Late4dTrain.Domain.Abstractions/IAggregateRoot.cs:3-5`: the contract is tied to an event type rather than expressing an identity-bearing aggregate boundary.
- `Late4dTrain.Domain.Abstractions.Tests/Entity/EqualityTests.cs:40,76`: unequal entities are incorrectly required to have unequal hashes, although collisions are legal.
- `README.md:22-32,67-73`: the aggregate example lacks the required `Id` override; implementing `IValueObject` alone does not supply value equality.
- `DomainEvent` has no explicit occurrence-time constructor. There are no invariant-focused consumer examples or structural enforcement tools.

This library currently covers portions of tactical DDD, not all of DDD. A framework can enforce selected structural rules, but it cannot infer business invariants, identify all aggregate boundaries, or guarantee ubiquitous language and bounded-context design.

## Execution order

| Plan | Scope | Dependencies |
| --- | --- | --- |
| [01 — Baseline](01-contract-baseline.md) | Required | Existing initialization |
| [02 — Value objects](02-value-objects.md) | Required | 01 |
| [03 — Entity identity](03-entity-identity.md) | Required | 01, 02 for typed-ID example |
| [04 — Aggregates and events](04-aggregate-event-ownership.md) | Required | 03 |
| [05 — Domain events](05-domain-events.md) | Required | 04 |
| [06 — Reference domain](06-invariants-reference-domain.md) | Required | 02–05 |
| [07 — Consumer enforcement](07-consumer-enforcement.md) | Required for structural-enforcement goal | 02–06 |
| [08 — Repository ports](08-repository-ports.md) | Optional | 04, 06 |
| [09 — Specifications](09-specifications.md) | Optional | 02, 06 |
| [10 — Release readiness](10-release-readiness.md) | Required before release | 01–07; include 08/09 if selected |

Complete each increment and its verification before starting the next. The core path is 01 → 02 → 03 → 04 → 05 → 06 → 07 → 10; optional plans 08 and 09 must not become hidden prerequisites.

## Shared constraints and verification

- Keep the library on `netstandard2.0` and C# 10 unless a later approved scope changes them. Tests use .NET 10, xUnit, BDDfy, AutoFixture/AutoData, and FluentAssertions.
- Preserve existing uncommitted work; ask before unsafe branching. Do not commit, publish, or discard changes without authorization.
- From the repository root run `dotnet restore Late4dTrain.Domain.Abstractions.sln`, `dotnet build Late4dTrain.Domain.Abstractions.sln --no-restore`, then `dotnet test Late4dTrain.Domain.Abstractions.sln --no-build --verbosity normal`.
- Use focused test filters while developing. Run `dotnet format Late4dTrain.Domain.Abstractions.sln --verify-no-changes --no-restore --include <changed-csharp-files>`; report existing unrelated formatting drift separately.
- Track acceptance criteria and actual verification in each plan. Do not claim that a passing baseline proves unimplemented behavior.

## References

- [Value objects and immutability](https://martinfowler.com/bliki/ValueObject.html); [aggregate roots](https://martinfowler.com/bliki/DDD_Aggregate.html); [bounded contexts](https://martinfowler.com/bliki/BoundedContext.html); [anemic domain models](https://martinfowler.com/bliki/AnemicDomainModel.html); [repositories](https://martinfowler.com/eaaCatalog/repository.html).
- [.NET domain-model validation](https://learn.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-model-layer-validations); [.NET domain events and dispatch timing](https://learn.microsoft.com/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation).
- These references do not require EF, MediatR, microservices, or the exact example APIs.
