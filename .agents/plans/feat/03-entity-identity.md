# 03 — Stable entity identity

Status: implemented. Required. Dependencies: [01](01-contract-baseline.md), [02](02-value-objects.md) for typed-ID examples.

## Objective and contract

Require a protected constructor with an ID; expose non-virtual get-only identity and reject null at runtime. Compare by exact concrete type and immutable ID, consistently across typed/object equality, operators and hash codes. Require identity at creation rather than supporting unassigned persistence-only entities.

## Implementation

- [x] Implement constructor-owned identity; forward it through the aggregate constructor and update fixtures.
- [x] Do not reject `default(TId)` universally: an explicitly assigned zero may be valid. Demonstrate consumer-owned `OrderId` and `CustomerId` with their own validation.
- [x] Document that reference-type IDs must also be immutable; generic constraints cannot prove this.
- [x] Cover equal IDs, different types, typed-ID separation, null rejection, allowed defaults, invalid ID construction, collision handling, and equality/operator symmetry.
- [x] Verify changing non-identity state does not break hash-collection lookups; compile a consumer example confirming the base exposes no setter/override path.

## Acceptance and verification

The base entity has a stable ID at construction and a single documented equality policy. ORM proxy equality and late database-generated IDs require a separate future adapter; no universal ID generator is introduced.

Verification: solution build succeeded with 0 warnings/errors; focused entity equality tests passed 16/16; full solution suite passed 29/29; focused format verification passed.
