# 02 — Explicit, reliable value-object equality

Status: implemented. Required. Dependency: [01](01-contract-baseline.md). Read the [roadmap](00-ddd-review-roadmap.md).

## Objective and contract

Replace reflection with `protected abstract IEnumerable<object?> GetEqualityComponents()`. Use the same components for typed/object equality, operators and hashing, with exact runtime-type equality. Remove cached hashes and make base equality/hash overrides resistant to inconsistent subclass overrides. Keep `IValueObject` as a classification marker for independently implemented value types; it does not itself provide equality.

## Implementation

Implementation notes: the core now uses an explicit ordered component sequence, `IEquatable<ValueObject>`, sealed object equality/hash overrides, and no hash cache. Focused edge-case tests and final verification remain.

- [x] Implement explicit equality components using only `netstandard2.0` APIs; update fixtures and examples.
- [x] Specify null and empty component semantics; empty components make same-concrete-type instances equal.
- [x] Document immutable components and explain that recomputing a hash does not make mutation safe while used as a collection key.
- [x] Demonstrate an immutable collection-valued object with defensive copying and explicit ordered element semantics. A collection yielded as a single component keeps its own equality semantics; do not silently deep-compare graphs.
- [x] Add tests for nulls, different concrete types, collisions, collection ordering/content, defensive copying, and dictionary/set lookup.
- [x] Add an unrelated computed getter or indexer that throws if invoked; equality must not invoke it unless explicitly included.

## Acceptance and verification

Equal instances always hash equally; all equality entrypoints agree; arbitrary getters are never discovered. No reflection fallback or generic graph comparer is included.

Verification: build succeeded with 0 warnings/errors; focused value-object tests passed 12/12; full solution tests passed 24/24; focused format verification passed after applying repository CRLF/BOM formatting to changed C# files.
