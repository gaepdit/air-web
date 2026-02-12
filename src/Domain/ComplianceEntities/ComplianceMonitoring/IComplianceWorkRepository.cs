using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.Comments;
using AirWeb.Domain.Lookups.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public interface IComplianceWorkRepository : IRepositoryWithMapping<ComplianceWork, int>
{
    public static string[] IncludeComments => [nameof(ComplianceWork.Comments)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    /// <summary>
    /// Returns the <see cref="ComplianceWork"/> with the given <paramref name="id"/> converted to the specified
    /// <see cref="TWork"/> type. Returns null if no entity exists with the given ID.
    /// The returned entity will include the Comments navigation property.
    /// </summary>
    /// <param name="id">The ID of the ComplianceWork.</param>
    /// <param name="includeExtras">Whether to include the <see cref="ComplianceWorkComment"/> and
    /// <see cref="ComplianceWorkAuditPoint"/> navigation properties with the result.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A ComplianceWork entry of type TWork or null.</returns>
    Task<TWork?> FindAsync<TWork>(int id, bool includeExtras, CancellationToken token = default)
        where TWork : ComplianceWork;

    /// <summary>
    /// Gets the <see cref="ComplianceWorkType"/> for the <see cref="ComplianceWork"/> with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the entry to look up.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The <see cref="ComplianceWorkType"/> of the entry.</returns>
    Task<ComplianceWorkType> GetComplianceWorkTypeAsync(int id, CancellationToken token = default);

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
