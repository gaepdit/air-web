namespace IaipDataService.Facilities;

public interface IFacilityService
{
    Task<Facility> GetAsync(FacilityId id);
    Task<Facility?> FindAsync(FacilityId? id);
    Task<string> GetNameAsync(string id);
    Task<bool> ExistsAsync(FacilityId id);

    // TODO: Remove later. This is only for testing. 
    //       This method is only used to provide a short list of test facilities and won't be used in
    //       the production version.
    Task<IReadOnlyCollection<Facility>> GetListAsync();
}
