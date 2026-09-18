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
    /// <param name="token"></param>
    Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(FacilityId facilityId, int skip, int take,
        CancellationToken token = default);

    /// <summary>
    /// Searches for permits by partial facility name.
    /// </summary>
    /// <param name="name">A partial facility name to search for.</param>
    /// <param name="permit">A partial permit number (or SIC code) to search for.</param>
    /// <param name="dateFrom">A starting permit issuance date to search from.</param>
    /// <param name="dateTo">An ending permit issuance date to search through.</param>
    /// <param name="skip">The number of permits search results to skip (for pagination).</param>
    /// <param name="take">The number of permits search results to take (for pagination).</param>
    Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(string? name, string? permit, DateOnly? dateFrom,
        DateOnly? dateTo, int skip, int take);

    /// <summary>
    /// Counts the number of permits for a facility.
    /// </summary>
    /// <param name="facilityId">A Facility ID.</param>
    Task<int> CountPermitsAsync(FacilityId facilityId);

    /// <summary>
    /// Counts the number of permits by searching by partial facility name.
    /// </summary>
    /// <param name="name">A partial facility name to search for.</param>
    /// <param name="permit">A partial permit number (or SIC code) to search for.</param>
    /// <param name="dateFrom">A starting permit issuance date to search from.</param>
    /// <param name="dateTo">An ending permit issuance date to search through.</param>
    Task<int> CountPermitsAsync(string? name, string? permit, DateOnly? dateFrom, DateOnly? dateTo);

    /// <summary>
    /// Get a permit document.
    /// </summary>
    /// <param name="fileName">The ID of the permit document.</param>
    /// <returns>A byte array of the permit document.</returns>
    Task<byte[]?> GetPermitFileAsync(string fileName);
}
