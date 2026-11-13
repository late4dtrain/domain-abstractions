# 10 — First-release readiness

Status: implemented. Required before publishing. Dependencies: [01](01-contract-baseline.md)–[07](07-consumer-enforcement.md); include [08](08-repository-ports.md) and [09](09-specifications.md) only if selected.

## Objective

Verify the selected library and analyzer contracts as independently consumed packages.

## Implementation

- [x] Publish a concise guarantee matrix distinguishing runtime contracts, optional diagnostics, consumer tests, and domain-modeling responsibilities.
- [x] Align README, XML documentation, and `AGENTS.md` with implemented APIs and toolchain; compile examples.
- [x] Build and pack runtime/analyzer packages, then restore and exercise them in a separate clean consumer project.
- [x] Confirm `netstandard2.0` compatibility and no unintended test, Roslyn-host, ORM, or transport dependencies in runtime packages.
- [x] Update CI/release workflows to supported action versions and require selected verification before publication.
- [x] Verify strict analyzer behavior on positive and negative consumer examples and inspect package contents.

## Acceptance and verification

A clean consumer can restore and use the packages, strict diagnostics reject representative violations, and CI validates the selected features. Do not publish without separate authorization. No migration guide or legacy compatibility layer is needed for the unreleased package.

Verification: solution build succeeded with 0 warnings/errors; final full suite passed 63/63; focused formatting verification passed; both packages packed and inspected (runtime package has `lib/netstandard2.0` plus README/guarantees and no dependencies; analyzer package has `analyzers/dotnet/cs` plus README and no package dependencies); a fresh-package consumer using both NuGet packages built with 0 warnings/errors; strict consumer check failed as expected with `error L4DA001`. CI/release workflows now build, test, pack, validate the consumer and strict diagnostic before publishing. No package was published.
