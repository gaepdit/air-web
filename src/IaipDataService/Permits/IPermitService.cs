namespace IaipDataService.Permits;

public interface IPermitService
{
    /// <summary>
    /// Searches for permits for a facility ID or partial facility name.
    /// </summary>
    /// <param name="facilityId">A Facility ID.</param>
    /// <param name="name">The facility name to search for.</param>
    /// <param name="skip">The number of permits search results to skip (for pagination).</param>
    /// <param name="take">The number of permits search results to take (for pagination).</param>
    Task<IReadOnlyCollection<PermitSummary>> GetPermitListAsync(string? facilityId, string? name, int skip, int take);

    /// <summary>
    /// Counts the number of permits for a facility ID or partial facility name.
    /// </summary>
    /// <param name="facilityId">A Facility ID.</param>
    /// <param name="name">The facility name to search for.</param>
    Task<int> CountPermitsAsync(string? facilityId, string? name);

    /// <summary>
    /// Get a permit document.
    /// </summary>
    /// <param name="fileName">The ID of the permit document.</param>
    /// <returns>A byte array of the permit document.</returns>
    Task<byte[]?> GetPermitFileAsync(string fileName);
}
