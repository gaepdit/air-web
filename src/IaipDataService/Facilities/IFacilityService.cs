using System.Collections.ObjectModel;

namespace IaipDataService.Facilities;

public interface IFacilityService
{
    Task<Facility> GetAsync(FacilityId id, bool forceRefresh = false);
    Task<Facility?> FindAsync(FacilityId? id, bool forceRefresh = false);
    Task<string> GetNameAsync(string id);
    Task<bool> ExistsAsync(FacilityId id);

    /// <summary>
    /// Retrieves a list of facilities as a Dictionary of Facility IDs and Facility names.
    /// </summary>
    Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync(bool forceRefresh = false);
}
