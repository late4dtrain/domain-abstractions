namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// Boolean composition helpers for specifications.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Combines two specifications with short-circuiting logical AND.
    /// </summary>
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));

        return new PredicateSpecification<T>(candidate => left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate));
    }

    /// <summary>
    /// Combines two specifications with short-circuiting logical OR.
    /// </summary>
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
    {
        if (left is null) throw new ArgumentNullException(nameof(left));
        if (right is null) throw new ArgumentNullException(nameof(right));

        return new PredicateSpecification<T>(candidate => left.IsSatisfiedBy(candidate) || right.IsSatisfiedBy(candidate));
    }

    /// <summary>
    /// Negates a specification.
    /// </summary>
    public static ISpecification<T> Not<T>(this ISpecification<T> specification)
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));

        return new PredicateSpecification<T>(candidate => !specification.IsSatisfiedBy(candidate));
    }

    private sealed class PredicateSpecification<T> : ISpecification<T>
    {
        private readonly Func<T, bool> _predicate;

        public PredicateSpecification(Func<T, bool> predicate) => _predicate = predicate;

        public bool IsSatisfiedBy(T candidate) => _predicate(candidate);
    }
}
