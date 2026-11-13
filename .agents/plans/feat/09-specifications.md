# 09 — Composable business specifications

Status: implemented, optional. Dependencies: [02](02-value-objects.md), [06](06-invariants-reference-domain.md).

## Objective

Support reusable pure business predicates without coupling them to persistence queries.

## Implementation

- [x] Provide a small `IsSatisfiedBy(T candidate)` contract with `And`, `Or`, and `Not` composition.
- [x] Define null handling and short-circuit behavior.
- [x] Demonstrate a real eligibility rule and test composition, short-circuiting, and no mutation or I/O.
- [x] Keep expression-tree/query translation separate unless an actual repository adapter requires it.

## Acceptance and verification

Composition follows Boolean semantics, tests prove evaluation behavior, and the example does not bypass aggregate invariants. A specification is optional; not every validation requires this pattern.

Verification: solution build succeeded with 0 warnings/errors; focused specification tests passed 4/4; full solution suite passed 62/62; focused formatting verification passed.
