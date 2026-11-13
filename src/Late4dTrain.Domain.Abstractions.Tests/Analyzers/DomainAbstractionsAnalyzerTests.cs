namespace Late4dTrain.Domain.Abstractions.Tests.Analyzers;

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using Late4dTrain.Domain.Abstractions.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

using Xunit;

public class DomainAbstractionsAnalyzerTests
{
    [Fact]
    public async Task Reports_Structural_Violations_Across_Direct_And_Indirect_Contracts()
    {
        const string source = Framework + """
            namespace Consumer;
            using System.Collections.Generic;
            using Late4dTrain.Domain.Abstractions;
            public sealed class MutableValue : ValueObject { public int Amount { get; private set; } }
            public sealed class MutableEvent : DomainEvent { public string Name { get; set; } = ""; }
            public sealed class MutableDirectEvent : IDomainEvent { public System.DateTimeOffset DateOccurred { get; } public int Sequence { get; set; } }
            public sealed class MutableIdentity : IEntity<int> { public int Id { get; set; } }
            public sealed class Root : IAggregateRoot { public List<int> Items { get; } = new(); }
            public sealed class MarkerOnly : IValueObject { }
            public abstract class MutableValueBase : ValueObject { public int Level { get; set; } }
            public sealed class DerivedMutableValue : MutableValueBase { }
            public interface ICustomValue : IValueObject { }
            public sealed class IndirectMarker : ICustomValue { }
            """;

        var diagnostics = await Analyze(source);

        Assert.Equal(8, diagnostics.Length);
        Assert.Equal(2, diagnostics.Count(diagnostic => diagnostic.Id == DomainAbstractionsAnalyzer.ImmutableValueObjectId));
        Assert.Equal(2, diagnostics.Count(diagnostic => diagnostic.Id == DomainAbstractionsAnalyzer.ImmutableDomainEventId));
        Assert.Contains(diagnostics, diagnostic => diagnostic.Id == DomainAbstractionsAnalyzer.ReadOnlyEntityIdentityId);
        Assert.Contains(diagnostics, diagnostic => diagnostic.Id == DomainAbstractionsAnalyzer.AggregateCollectionId);
        Assert.Equal(2, diagnostics.Count(diagnostic => diagnostic.Id == DomainAbstractionsAnalyzer.ValueObjectEqualityId));
        var mutableStateDiagnostic = diagnostics.First(diagnostic =>
            diagnostic.Id == DomainAbstractionsAnalyzer.ImmutableValueObjectId);
        var diagnosticLine = mutableStateDiagnostic.Location.GetLineSpan().StartLinePosition.Line;
        Assert.Contains("set", source.Split('\n')[diagnosticLine]);
    }

    [Fact]
    public async Task Ignores_Immutable_Inherited_And_Record_ValueObjects()
    {
        const string source = Framework + """
            namespace Consumer;
            using Late4dTrain.Domain.Abstractions;
            public class ImmutableValue : ValueObject { public int Amount { get; } }
            public sealed class DerivedValue : ImmutableValue { }
            public sealed record RecordValue(int Amount) : IValueObject;
            public sealed class ImmutableEvent : DomainEvent { public string Name { get; } = ""; }
            public sealed class ImmutableIdentity : IEntity<int> { public int Id { get; } }
            public sealed class Root : IAggregateRoot { public System.Collections.Generic.IReadOnlyList<int> Items { get; } = System.Array.Empty<int>(); }
            """;

        var diagnostics = await Analyze(source);

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task Honors_Pragma_Suppression()
    {
        const string source = Framework + """
            namespace Consumer;
            using Late4dTrain.Domain.Abstractions;
            #pragma warning disable L4DA001
            public sealed class SuppressedValue : ValueObject { public int Amount { get; set; } }
            #pragma warning restore L4DA001
            """;

        var diagnostics = await Analyze(source);

        Assert.Empty(diagnostics);
    }

    private const string Framework = """
        namespace Late4dTrain.Domain.Abstractions
        {
            public interface IValueObject { }
            public abstract class ValueObject : IValueObject { }
            public interface IDomainEvent { System.DateTimeOffset DateOccurred { get; } }
            public abstract class DomainEvent : IDomainEvent { public System.DateTimeOffset DateOccurred { get; } }
            public interface IEntity<TId> { TId Id { get; } }
            public interface IAggregateRoot { }
        }
        """;

    private static async Task<ImmutableArray<Diagnostic>> Analyze(string source)
    {
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(System.IO.Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(
            "AnalyzerTest",
            new[] { CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Latest)) },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var analyzer = new DomainAbstractionsAnalyzer();
        return await compilation.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(analyzer)).GetAnalyzerDiagnosticsAsync();
    }
}
