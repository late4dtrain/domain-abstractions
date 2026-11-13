# 07 — Consumer-side structural enforcement

Status: implemented. Required for the stated enforcement goal. Dependencies: [02](02-value-objects.md)–[06](06-invariants-reference-domain.md).

## Objective

Provide opt-in, actionable compile-time diagnostics for observable structural violations without coupling runtime code to Roslyn.

## Implementation

- [x] Add a separate analyzer package with documented IDs, default severities, examples, and a strict consumer profile treating selected diagnostics as errors.
- [x] Diagnose visible writable value-object/event state, writable identity on direct entity implementations, exposed mutable aggregate collections, and marker implementations lacking suitable equality where reliably observable.
- [x] Use semantic symbol analysis; handle inherited members, direct/indirect interfaces, valid record equality, generated code, and configurable severity. Avoid claiming detection of cases that cannot be determined reliably.
- [x] Test positive and negative snippets, diagnostic locations, inherited cases, suppression, strict configuration, and false positives.
- [x] Validate NuGet analyzer packaging through a separate consumer project; document required compiler compatibility.
- [x] Show consumer-owned architecture checks for explicitly declared project/dependency boundaries.

## Acceptance and verification

A separate consumer fails under the strict profile for representative violations, while valid domain examples compile cleanly. Analyzer tests use semantic compilations; package-consumer validation confirms NuGet delivery rather than only project references.

Verification: solution build succeeded with 0 warnings/errors; full test suite passed 53/53; focused format verification passed; analyzer package packed successfully and contains its assembly under `analyzers/dotnet/cs` plus README; the clean consumer sample built with 0 warnings/errors; adding a deliberately mutable `ValueObject` made that consumer fail with `error L4DA001` under `.editorconfig`.

## Risks

Static diagnostics cannot prove deep immutability, business correctness, or arbitrary aggregate membership. Prefer narrow reliable rules over broad misleading warnings.
