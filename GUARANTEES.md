# Contract and guarantee matrix

| Area | What this repository provides | What it does not guarantee |
| --- | --- | --- |
| Entity identity | `Entity<TId>` accepts an ID at construction, exposes it read-only, and compares same-runtime-type entities by ID. | Immutability of arbitrary reference-type IDs, ORM proxy behavior, or late-generated database IDs. |
| Value objects | `ValueObject` compares and hashes the explicitly ordered `GetEqualityComponents()` sequence. | Deep immutability, graph comparison, or properties that the consumer omits from the component sequence. |
| Aggregate roots and events | `AggregateRoot<TId>` owns a heterogeneous pending-event buffer, returns read-only snapshots, and acknowledges captured occurrences. | Durable delivery, dispatch, transactions, retries across process failure, or exactly-once semantics. |
| Domain events | Event occurrence time is fixed at construction and normalized to UTC. | Integration-message transport, event sourcing, or atomic publication with persistence. |
| Repository ports (optional) | `IAggregateRepository<TAggregate,TId>` constrains aggregate type, identity, missing-result and cancellation contracts. | Storage implementation, optimistic concurrency, shared unit of work, or cross-aggregate atomicity. |
| Specifications (optional) | Pure-by-convention predicates with short-circuiting Boolean composition. | Purity enforcement, I/O prohibition, or translation to database queries. |
| Analyzer package (optional) | Warnings for selected structural patterns, configurable to errors in consumer `.editorconfig`. | Proof of business correctness, deep immutability, bounded contexts, or aggregate boundaries. |
| Consumer responsibilities | Tests and examples demonstrate selected domain invariants and architecture assertions. | Correct ubiquitous language, invariants, deployment, persistence consistency, and architecture for every consuming application. |

The runtime library targets `netstandard2.0` and has no explicit package dependencies. Analyzer/compiler dependencies are isolated in `Late4dTrain.Domain.Abstractions.Analyzers`.
