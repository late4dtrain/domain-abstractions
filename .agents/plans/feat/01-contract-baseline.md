# 01 — Establish reliable contracts and tests

Status: implemented. Required. Dependency: existing initialization. Read [roadmap](00-ddd-review-roadmap.md) first.

## Objective

Make tests and documentation a trustworthy baseline before changing runtime APIs.

## Implementation

- [x] Document first-release scope: tactical DDD building blocks and consumer analyzers; runtime `netstandard2.0`, tests `net10.0`.
- [x] Correct the two assertions requiring unequal entities to have unequal hashes; retain equal-object/equal-hash checks.
- [x] Add an immutable custom ID with deliberate collisions and verify distinct entities remain retrievable in `HashSet` and `Dictionary`.
- [x] Cover same-reference equality, one-null comparisons in both orders, unrelated objects, and concrete-type differences.
- [x] Fix the README aggregate `Id` override and replace the marker-only value-object example with a real `ValueObject` implementation.
- [x] Compile example source as consumer fixtures; do not merely search README text.
- [x] Use xUnit, BDDfy, AutoFixture and AutoData; explicitly supply null/default/collision edge cases.

## Acceptance and verification

All baseline scenarios pass; hash collisions are supported; documented examples compile. Run focused equality scenarios and the shared checks. Do not redesign runtime APIs or check in intentionally failing future-contract tests in this increment.

Verification: `dotnet build Late4dTrain.Domain.Abstractions.slnx --no-restore` succeeded with 0 warnings/errors; `dotnet test Late4dTrain.Domain.Abstractions.slnx --no-build --verbosity normal` passed all 19 tests; focused equality tests passed 18/18; focused domain-event test passed 1/1; focused `dotnet format ... --verify-no-changes` passed. `git diff --check` reports CR characters on CRLF additions in the pre-existing CRLF-configured `DomainEventTests.cs`; this is line-ending behavior, not spaces in source.
