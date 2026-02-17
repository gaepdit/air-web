namespace AirWeb.Domain.Compliance.ComplianceEntities.Fces;

public interface IFceRepository : IRepositoryWithMapping<Fce, int>
{
    public static string[] IncludeComments => [nameof(Fce.Comments)];

    // Will return the next available ID if the repository requires it for adding new entities (e.g., local repository).
    // Will return null if the repository creates a new ID on insert (e.g., Entity Framework).
    int? GetNextId();

    /// <summary>
    /// Returns the <see cref="Fce"/> with the given <paramref name="id"/>. Returns null if there are no matches.
    /// The returned entity will include the Comments and Audit Points navigation properties.
    /// </summary>
    /// <param name="id">The ID of the FCE.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An FCE with Comments included or null.</returns>
    Task<Fce?> FindWithDetailsAsync(int id, CancellationToken token = default);

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
}
