namespace IaipDataService.Facilities;

public interface IFacilityService
{
    Task<Facility> GetAsync(FacilityId id, CancellationToken token = default);
    Task<Facility?> FindAsync(FacilityId? id);
    Task<bool> ExistsAsync(FacilityId id);

    // TODO: Remove later. This is only for testing. 
    //       This method is only used to provide a short list of test facilities and won't be used in
    //       the production version.
    Task<IReadOnlyCollection<Facility>> GetListAsync();
}
