# 08 — Aggregate-root repository ports

Status: implemented, optional. Dependencies: [04](04-aggregate-event-ownership.md), [06](06-invariants-reference-domain.md).

## Objective

Offer minimal domain-facing persistence contracts without embedding storage technologies.

## Implementation

- [x] Design root-constrained repository contracts with explicit identity, missing-result, cancellation, and persistence semantics.
- [x] Demonstrate a domain-specific `IOrderRepository` extension and separate read-model/query projections.
- [x] Keep `IQueryable`, database types, ORM, and transport dependencies out of the domain contract.
- [x] Add a unit-of-work abstraction only if the reference application demonstrates a shared commit boundary requiring it; no generic unit-of-work was added because this library has no persistence adapter or shared commit boundary.
- [x] Exercise lookup, missing results, cancellation and root persistence with in-memory fakes.

## Acceptance and verification

Compile-time constraints reject ordinary child entities and value objects; consumers can implement persistence without infrastructure references in the domain. Generic CRUD alone does not prove aggregate consistency or transactional behavior.

Verification: solution build succeeded with 0 warnings/errors; focused repository tests passed 5/5; full suite passed 58/58; focused formatting verification passed. Operations are independent persistence calls; no multi-aggregate transaction guarantee is made.
