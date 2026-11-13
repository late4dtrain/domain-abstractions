namespace Late4dTrain.Domain.Abstractions;

/// <summary>
/// A pure business predicate over a candidate.
/// </summary>
/// <typeparam name="T">The candidate type.</typeparam>
/// <remarks>
/// Implementations should be deterministic and free of I/O or mutation. The contract does not enforce purity.
/// Null candidates are passed through to the implementation; candidate null handling is domain-specific.
/// </remarks>
public interface ISpecification<in T>
{
    /// <summary>
    /// Determines whether the candidate satisfies this specification.
    /// </summary>
    bool IsSatisfiedBy(T candidate);
}
