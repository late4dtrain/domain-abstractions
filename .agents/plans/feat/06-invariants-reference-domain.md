# 06 — Invariants and a rich reference domain

Status: implemented. Required. Dependencies: [02](02-value-objects.md)–[05](05-domain-events.md).

## Objective

Demonstrate correct use of the framework in a compilable consuming domain, not merely through marker interfaces.

Implementation notes: a test-project consumer sample now includes typed IDs, `Money`, an `Order` aggregate, owned `OrderLine` entities, and line-added/submitted events. There are no invented factory or domain-service abstractions because these rules need no external collaboration. A multilayer architecture test is not applicable to this single-library repository; enforce such boundaries in a real consumer solution.

## Implementation

- [x] Create a small `Order` aggregate with owned line-item entities, immutable money and strongly typed IDs, and named operations such as adding items and submitting.
- [x] Enforce positive quantities, compatible currencies, submission requirements, and restricted changes after submission.
- [x] Validate before mutation so failed operations leave state and pending events unchanged.
- [x] Demonstrate a factory only where valid construction requires collaboration, and a domain service only for a rule genuinely spanning concepts.
- [x] Reference other aggregate roots by identity; encapsulate child collections and operations.
- [x] Assess architecture-test scope; marked not applicable because the repository has no application/infrastructure project boundary to enforce.
- [x] Cover valid/invalid creation and transitions using xUnit, BDDfy, AutoFixture/AutoData, and FluentAssertions.

## Acceptance and verification

Invalid state cannot be created through the example's public API; failed transitions do not mutate state or emit events; external callers cannot bypass owned-child invariants. This repository contains only a library and test project, so no artificial multi-layer architecture test was introduced; the sample directly verifies aggregate/event boundaries.

Verification: solution build succeeded with 0 warnings/errors; focused reference-domain tests passed 10/10; final full solution suite passed 63/63; focused format verification passed.

## Boundaries

Do not add empty domain-service or generic factory interfaces merely to represent terminology. Business rules and aggregate boundaries remain consumer decisions.
