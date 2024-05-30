using AirWeb.Domain.ValueObjects;
using System.Linq.Expressions;

namespace AirWeb.Domain.Entities.WorkEntries;

public interface IWorkEntryRepository : IRepository<BaseWorkEntry, int>

{
    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    // TODO: If this works, move to app library package.
    /// <summary>
    /// Returns the <see cref="BaseWorkEntry"/> with the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="includeProperties">The navigation properties to include (when using an Entity Framework repository).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown if no entity exists with the given Id.</exception>
    /// <returns>An entity.</returns>
    Task<BaseWorkEntry> GetAsync(int id, string[] includeProperties, CancellationToken token = default);

    // TODO: If this works, move to app library package.
    /// <summary>
    /// Returns the <see cref="BaseWorkEntry"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns null if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="includeProperties">The navigation properties to include (when using an Entity Framework repository).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>An entity or null.</returns>
    Task<BaseWorkEntry?> FindAsync(Expression<Func<BaseWorkEntry, bool>> predicate, string[] includeProperties,
        CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="IEntity{TKey}"/> with the given <paramref name="id"/>.
    /// Returns null if no entity exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An entity or null.</returns>
    Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default) where TEntry : BaseWorkEntry;

    /// <summary>
    /// Gets the <see cref="WorkEntryType"/> for the <see cref="BaseWorkEntry"/> with the specified Id.
    /// </summary>
    /// <param name="id">The Id of the Entry to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The <see cref="WorkEntryType"/> of the Entry.</returns>
    Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default);

    /// <summary>
    /// Gets the <see cref="WorkEntryType"/> for the <see cref="BaseComplianceEvent"/> with the specified Id.
    /// </summary>
    /// <param name="id">The Id of the Entry to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The <see cref="WorkEntryType"/> of the Entry.</returns>
    Task<ComplianceEventType> GetComplianceEventTypeAsync(int id, CancellationToken token = default);

    /// <summary>
    /// Adds a <see cref="Comment"/> to a <see cref="BaseWorkEntry"/>.
    /// </summary>
    /// <param name="id">The Id of the work entry.</param>
    /// <param name="comment">The comment to add.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task AddCommentAsync(int id, Comment comment, CancellationToken token = default);
}
