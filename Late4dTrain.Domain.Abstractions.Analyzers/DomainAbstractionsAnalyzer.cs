namespace Late4dTrain.Domain.Abstractions.Analyzers;

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DomainAbstractionsAnalyzer : DiagnosticAnalyzer
{
    public const string ImmutableValueObjectId = "L4DA001";
    public const string ImmutableDomainEventId = "L4DA002";
    public const string ReadOnlyEntityIdentityId = "L4DA003";
    public const string AggregateCollectionId = "L4DA004";
    public const string ValueObjectEqualityId = "L4DA005";

    private static readonly DiagnosticDescriptor ImmutableValueObjectRule = Rule(
        ImmutableValueObjectId,
        "Value object state should be immutable",
        "'{0}' exposes writable state; value objects should be immutable");

    private static readonly DiagnosticDescriptor ImmutableDomainEventRule = Rule(
        ImmutableDomainEventId,
        "Domain event state should be immutable",
        "'{0}' exposes writable state; domain events should be immutable");

    private static readonly DiagnosticDescriptor ReadOnlyEntityIdentityRule = Rule(
        ReadOnlyEntityIdentityId,
        "Entity identity should be read-only",
        "Entity identity '{0}' is writable");

    private static readonly DiagnosticDescriptor AggregateCollectionRule = Rule(
        AggregateCollectionId,
        "Aggregate collection should not be mutable to callers",
        "Aggregate property '{0}' exposes a mutable collection type");

    private static readonly DiagnosticDescriptor ValueObjectEqualityRule = Rule(
        ValueObjectEqualityId,
        "Marker value object should implement equality",
        "'{0}' implements IValueObject without value equality; inherit ValueObject or implement equality");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        ImmutableValueObjectRule,
        ImmutableDomainEventRule,
        ReadOnlyEntityIdentityRule,
        AggregateCollectionRule,
        ValueObjectEqualityRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
    }

    private static void AnalyzeNamedType(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol type || type.TypeKind != TypeKind.Class)
        {
            return;
        }

        var derivesValueObject = DerivesFrom(type, "Late4dTrain.Domain.Abstractions.ValueObject");
        var isDomainEvent = DerivesFrom(type, "Late4dTrain.Domain.Abstractions.DomainEvent") ||
            Implements(type, "Late4dTrain.Domain.Abstractions.IDomainEvent");
        var implementsValueObject = Implements(type, "Late4dTrain.Domain.Abstractions.IValueObject");
        var isAggregateRoot = Implements(type, "Late4dTrain.Domain.Abstractions.IAggregateRoot");
        var implementsEntity = type.AllInterfaces.Any(implemented =>
            implemented.OriginalDefinition.ToDisplayString() == "Late4dTrain.Domain.Abstractions.IEntity<TId>");

        foreach (var member in type.GetMembers())
        {
            if (member.IsStatic)
            {
                continue;
            }

            if (member is IPropertySymbol property)
            {
                if (derivesValueObject && property.SetMethod is not null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        ImmutableValueObjectRule,
                        property.SetMethod!.Locations.FirstOrDefault() ?? property.Locations.FirstOrDefault(),
                        property.Name));
                }

                if (isDomainEvent && property.SetMethod is not null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        ImmutableDomainEventRule,
                        property.SetMethod!.Locations.FirstOrDefault() ?? property.Locations.FirstOrDefault(),
                        property.Name));
                }

                if (implementsEntity && property.Name == "Id" && property.SetMethod is not null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        ReadOnlyEntityIdentityRule,
                        property.SetMethod!.Locations.FirstOrDefault() ?? property.Locations.FirstOrDefault(),
                        property.Name));
                }

                if (isAggregateRoot && IsPubliclyVisible(property) && IsMutableCollection(property.Type))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        AggregateCollectionRule,
                        property.Locations.FirstOrDefault(),
                        property.Name));
                }
            }
            else if (member is IFieldSymbol field && !field.IsImplicitlyDeclared && !field.IsReadOnly && !field.IsConst)
            {
                if (derivesValueObject)
                {
                    context.ReportDiagnostic(Diagnostic.Create(ImmutableValueObjectRule, field.Locations.FirstOrDefault(), field.Name));
                }

                if (isDomainEvent)
                {
                    context.ReportDiagnostic(Diagnostic.Create(ImmutableDomainEventRule, field.Locations.FirstOrDefault(), field.Name));
                }
            }
        }

        if (implementsValueObject && !type.IsAbstract && !derivesValueObject && !type.IsRecord && !OverridesEquality(type))
        {
            context.ReportDiagnostic(Diagnostic.Create(ValueObjectEqualityRule, type.Locations.FirstOrDefault(), type.Name));
        }
    }

    private static bool DerivesFrom(INamedTypeSymbol type, string metadataName)
    {
        for (var current = type.BaseType; current is not null; current = current.BaseType)
        {
            if (current.ToDisplayString() == metadataName)
            {
                return true;
            }
        }

        return false;
    }

    private static bool Implements(INamedTypeSymbol type, string metadataName) =>
        type.AllInterfaces.Any(implemented => implemented.ToDisplayString() == metadataName);

    private static bool IsPubliclyVisible(ISymbol symbol) =>
        symbol.DeclaredAccessibility == Accessibility.Public ||
        symbol.DeclaredAccessibility == Accessibility.Protected ||
        symbol.DeclaredAccessibility == Accessibility.ProtectedOrInternal ||
        symbol.DeclaredAccessibility == Accessibility.ProtectedAndInternal;

    private static bool IsMutableCollection(ITypeSymbol type)
    {
        if (type is IArrayTypeSymbol)
        {
            return true;
        }

        if (type is not INamedTypeSymbol namedType)
        {
            return false;
        }

        var definition = namedType.OriginalDefinition.ToDisplayString();
        return definition is
            "System.Collections.Generic.List<T>" or
            "System.Collections.Generic.IList<T>" or
            "System.Collections.Generic.ICollection<T>" or
            "System.Collections.Generic.ISet<T>" or
            "System.Collections.Generic.Dictionary<TKey, TValue>" or
            "System.Collections.Generic.IDictionary<TKey, TValue>" or
            "System.Collections.Generic.HashSet<T>";
    }

    private static bool OverridesEquality(INamedTypeSymbol type)
    {
        for (var current = type; current is not null && current.SpecialType != SpecialType.System_Object; current = current.BaseType)
        {
            var overridesEquals = current.GetMembers("Equals").OfType<IMethodSymbol>().Any(method =>
                method.IsOverride && method.Parameters.Length == 1 && method.Parameters[0].Type.SpecialType == SpecialType.System_Object);
            var overridesHashCode = current.GetMembers("GetHashCode").OfType<IMethodSymbol>().Any(method =>
                method.IsOverride && method.Parameters.Length == 0);
            if (overridesEquals && overridesHashCode) return true;
        }

        return false;
    }

    private static DiagnosticDescriptor Rule(string id, string title, string message) => new(
        id,
        title,
        message,
        "DomainModeling",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
}
