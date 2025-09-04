using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.NamedEntities.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public interface IWorkEntryRepository : IRepositoryWithMapping<WorkEntry, int>, ICommentRepository<int>
{
    public static string[] IncludeComments => [nameof(WorkEntry.Comments)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    /// <summary>
    /// Returns the <see cref="WorkEntry"/> with the given <paramref name="id"/> converted to the specified
    /// <see cref="TEntry"/> type. Returns null if no entity exists with the given ID.
    /// The returned entity will include the Comments navigation property.
    /// </summary>
    /// <param name="id">The ID of the WorkEntry.</param>
    /// <param name="includeExtras">Whether to include the <see cref="WorkEntryComment"/> and
    /// <see cref="WorkEntryAuditPoint"/> navigation properties with the result.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A Work Entry of type TEntry or null.</returns>
    Task<TEntry?> FindAsync<TEntry>(int id, bool includeExtras, CancellationToken token = default)
        where TEntry : WorkEntry;

    /// <summary>
    /// Gets the <see cref="WorkEntryType"/> for the <see cref="WorkEntry"/> with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the Entry to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The <see cref="WorkEntryType"/> of the Entry.</returns>
    Task<WorkEntryType> GetWorkEntryTypeAsync(int id, CancellationToken token = default);

    // Source test-specific

    /// <summary>
    /// Checks if a <see cref="SourceTestReview"/> exists with the given reference number.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the Source Test Review.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A boolean indicating whether the Source Test Review exists.</returns>
    Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default);

    /// <summary>
    /// Finds the <see cref="SourceTestReview"/> with the given Reference Number.
    /// </summary>
    /// <param name="referenceNumber">The reference number of the Source Test Review.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A <see cref="SourceTestReview"/> if found; otherwise, null.</returns>
    Task<SourceTestReview?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default);

    // Notification-specific

    /// <summary>
    /// Gets the <see cref="NotificationType"/> with the specified ID.
    /// </summary>
    /// <param name="typeId">The ID of the Notification Type to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown if no Notification Type exists with the given ID.</exception>
    /// <returns>The <see cref="NotificationType"/>.</returns>
    Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default);
}
