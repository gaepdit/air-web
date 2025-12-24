using System.Collections.ObjectModel;

namespace IaipDataService.Facilities;

public interface IFacilityService
{
    Task<Facility?> FindFacilityDetailsAsync(FacilityId id, bool forceRefresh = false);
    Task<Facility?> FindFacilitySummaryAsync(FacilityId id, bool forceRefresh = false);
    Task<string> GetNameAsync(string id);
    Task<bool> ExistsAsync(FacilityId id);

    /// <summary>
    /// Gets the current value for the next EPA action number for the given facility and then increments the saved value.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    Task<ushort> GetNextActionNumberAsync(FacilityId id);

    /// <summary>
    /// Retrieves a list of facilities as a Dictionary of Facility IDs and Facility names.
    /// </summary>
    Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync(bool forceRefresh = false);
}
