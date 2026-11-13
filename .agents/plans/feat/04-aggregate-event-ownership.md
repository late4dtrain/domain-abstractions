# 04 — Aggregate boundaries and pending-event ownership

Status: implemented. Required. Dependency: [03](03-entity-identity.md).

## Objective and contract

Introduce an event-independent root marker and `IAggregateRoot<TId> : IEntity<TId>, IAggregateRoot`. Prefer `AggregateRoot<TId>` recording heterogeneous `IDomainEvent` instances. Replace public mutable event lists with protected event recording and a genuinely read-only view or snapshot.

## Implementation

- [x] Introduce root identity contracts, constructor forwarding, null-checked protected `RaiseDomainEvent`, and a separate infrastructure-facing event lifecycle.
- [x] Make capture non-destructive. Acknowledge only captured occurrences by reference identity, preserving order and events raised after capture; specify repeated acknowledgement and duplicate occurrences.
- [x] Do not expose a backing `List<T>` as `IReadOnlyList<T>` where it can be downcast.
- [x] Update fixtures and README; do not dispatch or resolve services in the aggregate.
- [x] Test empty roots, multiple event types, insertion order, isolation, nulls, stable snapshots, duplicate references, and repeated acknowledgement.
- [x] Test failure without acknowledgement retains events, and events raised after capture survive acknowledgement.

## Acceptance and verification

Root identity is expressible independently of events; ordinary consumers cannot mutate the internal buffer; later events cannot be lost when earlier batches are acknowledged. Run aggregate/event-focused and shared checks.

## Boundaries

The API is in-memory bookkeeping, not a durable queue, transaction, or exactly-once delivery mechanism. Child event propagation and object-graph traversal require a concrete later use case.

Verification: build succeeded with 0 warnings/errors; focused aggregate/event tests passed 11/11; full solution suite passed 39/39; focused format verification passed.
