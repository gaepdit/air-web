using IaipDataService.Permits;

namespace IaipDataService.Facilities;

public interface IFacilityService
{
    /// <summary>
    /// Gets a summary of facility information for the facility with the given facility ID.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    /// <param name="forceRefresh">Whether to refresh cached data.</param>
    /// <param name="token"></param>
    Task<Facility?> FindFacilityAsync(FacilityId id, bool forceRefresh = false, CancellationToken token = default);

    /// <summary>
    /// Gets the name of the facility with the given facility ID.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    Task<string> GetNameAsync(string id);

    /// <summary>
    /// Returns whether a facility with the given facility ID exists.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    Task<bool> ExistsAsync(FacilityId id);

    /// <summary>
    /// Gets the current value for the next EPA action number for the given facility and then increments the saved value.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    Task<ushort> GetNextActionNumberAsync(FacilityId id);

    /// <summary>
    /// Gets the latest EPA Data Exchange date a given Facility.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    /// <param name="token"></param>
    Task<DateTime?> GetFacilityEpaDxDateAsync(FacilityId id, CancellationToken token = default);

    /// <summary>
    /// Sets the EPA Data Exchange status indicator to "Pending" (U).
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    Task<bool> RefreshEpaDataExchange(FacilityId id);

    /// <summary>
    /// Retrieves all facilities as a Facility summary.
    /// </summary>
    /// <param name="forceRefresh">Whether to refresh cached data.</param>
    /// <param name="includePortableSources">Whether to include Portable Sources (county code of "777").</param>
    /// <param name="token"></param>
    Task<IReadOnlyCollection<FacilitySummary>> GetAllAsync(bool forceRefresh = false,
        bool includePortableSources = true, CancellationToken token = default);

    /// <summary>
    /// Retrieves a list of all facilities with only names and IDs.
    /// </summary>
    /// <param name="token"></param>
    Task<IReadOnlyCollection<FacilityList>> GetListAsync(CancellationToken token = default);

    /// <summary>
    /// Searches for permits for a facility ID or partial facility name.
    /// </summary>
    /// <param name="facilityId">A Facility ID.</param>
    /// <param name="name">The facility name to search for.</param>
    /// <param name="skip">The number of permits search results to skip (for pagination).</param>
    /// <param name="take">The number of permits search results to take (for pagination).</param>
    /// <param name="token"></param>
    Task<IReadOnlyCollection<PermitSummary>> GetPermitListAsync(string? facilityId, string? name, int skip, int take,
        CancellationToken token = default);

    /// <summary>
    /// Counts the number of permits for a facility ID or partial facility name.
    /// </summary>
    /// <param name="facilityId">A Facility ID.</param>
    /// <param name="name">The facility name to search for.</param>
    /// <param name="token"></param>
    Task<int> CountPermitsAsync(string? facilityId, string? name, CancellationToken token = default);
}
