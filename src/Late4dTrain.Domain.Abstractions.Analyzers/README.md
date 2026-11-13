# Late4dTrain.Domain.Abstractions.Analyzers

This opt-in Roslyn analyzer package reports structural conventions for consumers of `Late4dTrain.Domain.Abstractions`. Diagnostics are warnings by default; the runtime library has no Roslyn dependency.

| ID | Default | Rule |
| --- | --- | --- |
| L4DA001 | Warning | Value-object properties or fields must not be writable. Applies to concrete classes deriving from `ValueObject`. |
| L4DA002 | Warning | Domain-event properties or fields must not be writable. Applies to `DomainEvent` subclasses and direct `IDomainEvent` implementations. |
| L4DA003 | Warning | Direct `IEntity<TId>` implementations must expose a read-only `Id`. Inherited `Entity<TId>` identity is already read-only. |
| L4DA004 | Warning | Aggregate-root properties must not expose arrays or known mutable collection types (`List<T>`, `IList<T>`, `ICollection<T>`, `ISet<T>`, dictionaries, and `HashSet<T>`). |
| L4DA005 | Warning | A concrete `IValueObject` implementation should inherit `ValueObject`, be a record, or override both `Equals(object)` and `GetHashCode()`. |

The rules inspect declared source members and semantic type relationships. Generated code is ignored. The analyzer targets `netstandard2.0` and uses Roslyn 4.11 APIs; consumers need a compiler host with Roslyn 4.11 or later. The analyzer cannot prove deep immutability, business correctness, or that a collection wrapper is truly immutable. It intentionally does not report arbitrary mutable reference-type properties because doing so would produce broad false positives.

## Strict consumer profile

Set selected diagnostics to errors in the consumer's `.editorconfig`:

```ini
[*.cs]
dotnet_diagnostic.L4DA001.severity = error
dotnet_diagnostic.L4DA002.severity = error
dotnet_diagnostic.L4DA003.severity = error
dotnet_diagnostic.L4DA004.severity = error
dotnet_diagnostic.L4DA005.severity = error
```

The [sample consumer project](https://github.com/late4dtrain/domain-abstractions/tree/main/samples/analyzer-consumer) demonstrates package consumption and this strict profile. Consumers should add architecture tests for project/dependency boundaries that are explicit in their own solution; this library cannot infer them.

For example, in a consumer test project, assert that a domain assembly does not reference an infrastructure assembly:

```csharp
[Fact]
public void Domain_does_not_depend_on_infrastructure()
{
    var referencedAssemblies = typeof(Order).Assembly.GetReferencedAssemblies();
    Assert.DoesNotContain(referencedAssemblies, assembly => assembly.Name == "MyApp.Infrastructure");
}
```

Adapt the assertion to actual consumer projects and permitted dependencies; the example is not part of the runtime package.
