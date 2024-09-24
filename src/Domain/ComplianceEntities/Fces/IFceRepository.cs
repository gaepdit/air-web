using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public interface IFceRepository : IRepository<Fce, int>
{
    public static string[] IncludeComments => [nameof(Fce.Comments)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    /// <summary>
    /// Returns the <see cref="Fce"/> with the given <paramref name="id"/>. Returns null if there are no matches.
    /// The returned entity will include the Comments navigation property.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An FCE with Comments included or null.</returns>
    Task<Fce?> FindWithCommentsAsync(int id, CancellationToken token = default);

    /// <summary>
    /// Returns a boolean indicating whether an <see cref="Fce"/> with the given parameters exists.
    /// </summary>
    /// <param name="facilityId">The ID of the facility.</param>
    /// <param name="year">The FCE year.</param>
    /// <param name="ignoreId">The ID of an FCE to ignore when evaluating the other conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>True if the FCE exists; otherwise false.</returns>
    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int? ignoreId = null,
        CancellationToken token = default);

    /// <summary>
    /// Adds a <see cref="Comment"/> to an <see cref="Fce"/>.
    /// </summary>
    /// <param name="itemId">The ID of the FCE.</param>
    /// <param name="comment">The comment to add.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task AddCommentAsync(int itemId, Comment comment, CancellationToken token = default);

    /// <summary>
    /// Deletes a comment from an <see cref="Fce"/>.
    /// </summary>
    /// <param name="commentId">The ID of the comment to delete.</param>
    /// <param name="userId"></param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default);
}
