namespace IaipDataService.Facilities;

public interface IFacilityService
{
    Task<Facility> GetAsync(FacilityId id, CancellationToken token = default);
    Task<Facility?> FindAsync(FacilityId? id);
    Task<bool> ExistsAsync(FacilityId id);

    // TODO: Remove later. This is only for testing. 
    Task<IReadOnlyCollection<Facility>> GetListAsync(CancellationToken token = default);
}
