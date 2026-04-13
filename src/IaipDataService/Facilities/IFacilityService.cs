namespace IaipDataService.Facilities;

public interface IFacilityService
{
    /// <summary>
    /// Gets a summary of facility information for the facility with the given facility ID.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    /// <param name="forceRefresh">Whether to refresh cached data.</param>
    Task<Facility?> FindFacilityAsync(FacilityId id, bool forceRefresh = false);

    /// <summary>
    /// Gets full facility details for the facility with the given facility ID.
    /// </summary>
    /// <param name="id">The Facility ID.</param>
    /// <param name="forceRefresh">Whether to refresh cached data.</param>
    Task<Facility?> FindFacilityDetailsAsync(FacilityId id, bool forceRefresh = false);

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
    /// Retrieves all facilities as a Facility summary.
    /// </summary>
    /// <param name="forceRefresh">Whether to refresh cached data.</param>
    Task<IReadOnlyCollection<FacilitySummary>> GetAllAsync(bool forceRefresh = false);
}
