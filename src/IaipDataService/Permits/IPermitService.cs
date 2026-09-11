using IaipDataService.Facilities;

namespace IaipDataService.Permits;

public interface IPermitService
{
    /// <summary>
    /// Searches for permits for a facility ID or partial facility name.
    /// </summary>
    /// <param name="facilityId">A Facility ID.</param>
    /// <param name="skip">The number of permits search results to skip (for pagination).</param>
    /// <param name="take">The number of permits search results to take (for pagination).</param>
    Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(FacilityId facilityId, int skip, int take);

    /// <summary>
    /// Searches for permits by partial facility name.
    /// </summary>
    /// <param name="name">The facility name to search for.</param>
    /// <param name="skip">The number of permits search results to skip (for pagination).</param>
    /// <param name="take">The number of permits search results to take (for pagination).</param>
    Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(string? name, int skip, int take);

    /// <summary>
    /// Counts the number of permits for a facility.
    /// </summary>
    /// <param name="facilityId">A Facility ID.</param>
    Task<int> CountPermitsAsync(FacilityId facilityId);

    /// <summary>
    /// Counts the number of permits by searching by partial facility name.
    /// </summary>
    /// <param name="name">The facility name to search for.</param>
    Task<int> CountPermitsAsync(string? name);

    /// <summary>
    /// Get a permit document.
    /// </summary>
    /// <param name="fileName">The ID of the permit document.</param>
    /// <returns>A byte array of the permit document.</returns>
    Task<byte[]?> GetPermitFileAsync(string fileName);
}
