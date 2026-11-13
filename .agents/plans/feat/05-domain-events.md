# 05 — Deterministic, immutable domain events

Status: implemented. Required. Dependency: [04](04-aggregate-event-ownership.md).

## Objective

Represent events as facts with immutable payloads and an explicit occurrence instant.

## Implementation

Implementation notes: explicit occurrence-time construction and UTC normalization are implemented.

- [x] Add a protected `DateTimeOffset` constructor to `DomainEvent`; store the instant once, normalized to UTC. A parameterless convenience constructor may use `DateTimeOffset.UtcNow`.
- [x] Keep `IDomainEvent.DateOccurred` get-only. Do not add a newer-runtime clock dependency to the `netstandard2.0` core.
- [x] Use meaningful immutable IDs and values in example events; defensively copy collection payloads.
- [x] Test explicit fixed timestamps and nonzero offsets; test convenience time with before/after bounds, not sleeps or exact clock equality.
- [x] Document dispatch timing relative to persistence and distinguish domain events from integration messages and event-sourcing records.

## Acceptance and verification

Historical occurrence time can be reproduced, event example payloads cannot change, and the core remains dependency-free. Event IDs, correlation IDs, versions and transports are optional integration concerns.

Verification: build succeeded with 0 warnings/errors; focused domain-event tests passed 3/3; full solution suite passed 41/41; focused format verification passed.
