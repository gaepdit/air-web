using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Entities.Fces;

public interface IFceRepository : IRepository<Fce, int>
{
    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

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
