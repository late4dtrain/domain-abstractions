namespace Late4dTrain.Domain.Abstractions;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Persistence port for one aggregate-root type.
/// </summary>
/// <remarks>
/// Each operation's commit and tracking behavior is defined by its implementation. This contract does not
/// establish a unit of work spanning multiple aggregates or atomicity with domain-event publication.
/// </remarks>
/// <typeparam name="TAggregate">The aggregate root type.</typeparam>
/// <typeparam name="TId">The aggregate identity type.</typeparam>
public interface IAggregateRepository<TAggregate, in TId>
    where TAggregate : class, IAggregateRoot<TId>
{
    /// <summary>
    /// Finds an aggregate by identity, returning <see langword="null"/> when it does not exist.
    /// </summary>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new aggregate. Implementations should reject an identity that already exists.
    /// </summary>
    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes to an existing aggregate. Implementations should report when its identity does not exist.
    /// </summary>
    Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
}
