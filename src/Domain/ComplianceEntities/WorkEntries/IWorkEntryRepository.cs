using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public interface IWorkEntryRepository : IRepository<WorkEntry, int>
{
    public static string[] IncludeComments => [nameof(WorkEntry.Comments)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    // TODO: Add unit tests for the following.

    // TODO: If this works (`string[] includeProperties`), move to app library package.
    /// <summary>
    /// Returns the <see cref="WorkEntry"/> with the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="includeProperties">The navigation properties to include (when using an Entity Framework repository).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown if no entity exists with the given ID.</exception>
    /// <returns>An entity.</returns>
    Task<WorkEntry> GetAsync(int id, string[] includeProperties, CancellationToken token = default);

    // TODO: If this works (`string[] includeProperties`), move to app library package.
    /// <summary>
    /// Returns the <see cref="WorkEntry"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns null if there are no matches.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="includeProperties">The navigation properties to include (when using an Entity Framework repository).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>An entity or null.</returns>
    Task<WorkEntry?> FindAsync(int id, string[] includeProperties, CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="WorkEntry"/> with the given <paramref name="id"/> converted to the specified
    /// <see cref="TEntry"/> type. Returns null if no entity exists with the given ID.
    /// </summary>
    /// <param name="id">The ID of the WorkEntry.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A Work Entry of type TEntry or null.</returns>
    Task<TEntry?> FindAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : WorkEntry;

    /// <summary>
    /// Returns the <see cref="WorkEntry"/> with the given <paramref name="id"/> converted to the specified
    /// <see cref="TEntry"/> type, including any <see cref="WorkEntry.Comments"/>.
    /// Returns null if no entity exists with the given ID.
    /// </summary>
    /// <param name="id">The ID of the WorkEntry.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A Work Entry of type TEntry or null.</returns>
    Task<TEntry?> FindWithCommentsAsync<TEntry>(int id, CancellationToken token = default)
        where TEntry : WorkEntry;

    /// <summary>
    /// Gets the <see cref="WorkEntryType"/> for the <see cref="WorkEntry"/> with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the Entry to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The <see cref="WorkEntryType"/> of the Entry.</returns>
    Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default);

    /// <summary>
    /// Gets the <see cref="ComplianceEventType"/> for the <see cref="ComplianceEvent"/> with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the Entry to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The <see cref="WorkEntryType"/> of the Entry.</returns>
    Task<ComplianceEventType> GetComplianceEventTypeAsync(int id, CancellationToken token = default);

    /// <summary>
    /// Gets the <see cref="NotificationType"/> with the specified ID.
    /// </summary>
    /// <param name="typeId">The ID of the Notification Type to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown if no Notification Type exists with the given ID.</exception>
    /// <returns>The <see cref="NotificationType"/>.</returns>
    Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default);

    /// <summary>
    /// Adds a <see cref="Comment"/> to a <see cref="WorkEntry"/>.
    /// </summary>
    /// <param name="id">The ID of the work entry.</param>
    /// <param name="comment">The comment to add.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task AddCommentAsync(int id, Comment comment, CancellationToken token = default);
}
