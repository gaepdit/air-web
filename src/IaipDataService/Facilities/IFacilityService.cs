using System.Collections.ObjectModel;

namespace IaipDataService.Facilities;

public interface IFacilityService
{
    Task<Facility> GetAsync(FacilityId id);
    Task<Facility?> FindAsync(FacilityId? id);
    Task<string> GetNameAsync(string id);
    Task<bool> ExistsAsync(FacilityId id);

    /// <summary>
    /// Retrieves a list of facilities as a Dictionary of Facility IDs and Facility names.
    /// </summary>
    Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync();
}
