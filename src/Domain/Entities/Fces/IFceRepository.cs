using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Entities.Fces;

public interface IFceRepository : IRepository<Fce, int>
{
    public static string[] IncludeComments => [nameof(Fce.Comments)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    // TODO: Add unit tests for the following.

    // TODO: If this works (`string[] includeProperties`), move to app library package.
    /// <summary>
    /// Returns the <see cref="Fce"/> with the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="includeProperties">The navigation properties to include (when using an Entity Framework repository).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown if no entity exists with the given ID.</exception>
    /// <returns>An entity.</returns>
    Task<Fce> GetAsync(int id, string[] includeProperties, CancellationToken token = default);

    // TODO: If this works (`string[] includeProperties`), move to app library package.
    /// <summary>
    /// Returns the <see cref="Fce"/> matching the given <paramref name="id"/>.
    /// Returns null if there are no matches.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="includeProperties">The navigation properties to include (when using an Entity Framework repository).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>An entity or null.</returns>
    Task<Fce?> FindAsync(int id, string[] includeProperties, CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="Fce"/> with the given <paramref name="id"/>, including any <see cref="Fce.Comments"/>.
    /// Returns null if there are no matches.
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
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>True if the FCE exists; otherwise false.</returns>
    public Task<bool> ExistsAsync(FacilityId facilityId, int year, CancellationToken token = default);

    /// <summary>
    /// Adds a <see cref="Comment"/> to an <see cref="Fce"/>.
    /// </summary>
    /// <param name="id">The ID of the FCE.</param>
    /// <param name="comment">The comment to add.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task AddCommentAsync(int id, Comment comment, CancellationToken token = default);
}
