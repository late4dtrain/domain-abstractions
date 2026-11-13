# Repository notes

## Library contracts

- `Late4dTrain.Domain.Abstractions` is the sole runtime NuGet library: `netstandard2.0`, C# 10, nullable enabled, no explicit package dependencies. The companion `Late4dTrain.Domain.Abstractions.Analyzers` package is separate and compiler-hosted; keep analyzer dependencies out of the runtime package. Tests target `net10.0`.
- `Entity<TId>` receives its ID through a protected constructor and exposes a non-virtual get-only property. Equality requires the same runtime type and equal IDs, including explicitly supplied default IDs; other properties are ignored. Reference-type IDs must be immutable. Aggregates forward IDs to the base constructor.
- `ValueObject` equality and hashing use the derived type's ordered `GetEqualityComponents()` sequence; public properties not yielded are ignored. Components and value objects should be immutable, especially when used as hash keys. `IValueObject` alone is only a marker and supplies no equality behavior.
- `AggregateRoot<TId>` records heterogeneous `IDomainEvent` instances through protected `RaiseDomainEvent`. `CaptureDomainEvents` returns a read-only snapshot; infrastructure acknowledges captured occurrences only after processing. This is in-memory bookkeeping, not durable delivery. `DomainEvent.DateOccurred` is captured once using `DateTimeOffset.UtcNow`.
- `IAggregateRepository<TAggregate, TId>` constrains domain persistence ports to aggregate roots; adapters define commit behavior. `ISpecification<T>` composition is pure by convention, short-circuiting, and separate from query translation.

## Verification

- CI uses .NET SDK `10.0.x`; there is no `global.json`. Test execution requires the .NET 10 runtime (`dotnet --list-runtimes`).
- Run from the repository root, in CI order:

  ```sh
  dotnet restore Late4dTrain.Domain.Abstractions.slnx
  dotnet build Late4dTrain.Domain.Abstractions.slnx --no-restore
  dotnet test Late4dTrain.Domain.Abstractions.slnx --no-build --verbosity normal
  ```

- Focused test after building: `dotnet test Late4dTrain.Domain.Abstractions.Tests --no-build --filter "FullyQualifiedName~After_TestAggregateRoot_Is_Created_DomainEvent_Is_Added"`. Both Entity and ValueObject suites have an `EqualityTests` class; qualify the namespace when filtering them.
- Tests use xUnit 2, BDDfy, AutoFixture `[AutoData]`, and FluentAssertions; follow `Entity/DomainEventTests.cs` for BDDfy and place fixtures in each suite's `Data/`. The `Tests.ValueObject` namespace shadows the library type, so fixtures use `using ValueObject = Abstractions.ValueObject;`.
- Formatting check after restore: `dotnet format Late4dTrain.Domain.Abstractions.slnx --verify-no-changes --no-restore`. Existing files have whitespace, CRLF and BOM drift against `.editorconfig`; use `--include <changed.cs>` for focused checks rather than reformatting the repository incidentally.
- Local packaging: `dotnet pack Late4dTrain.Domain.Abstractions.slnx -c Release`; inspect both runtime and analyzer `.nupkg` contents and build `samples/analyzer-consumer` against the local analyzer feed. The runtime package embeds `README.md` and `GUARANTEES.md`. `.github/workflows/release.yml` overrides package `Version` with the published release tag, not the checked-in `0.0.1`.

## OpenCode

- `opencode.jsonc` adapts the sibling Route66 setup: Plan by default, `.agents/plans/feat/*.md` as writable feature plans, and read-only `/review-uncommitted` and `/review-branch` commands. The feature workflow is loaded from `.opencode/instructions/feature-workflow.md`.
- The `route66` reference resolves to `../route66`; use it for workflow conventions, not as a library dependency. This repository's project files and tests govern framework and API decisions.
