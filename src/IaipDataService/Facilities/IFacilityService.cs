namespace IaipDataService.Facilities;

public interface IFacilityService : IDisposable, IAsyncDisposable
{
    Task<Facility> GetAsync(FacilityId id, CancellationToken token = default);
    Task<Facility?> FindAsync(FacilityId? id, CancellationToken token = default);
    Task<bool> ExistsAsync(FacilityId id, CancellationToken token = default);

    // TODO: Remove later. This is only for testing. 
    Task<IReadOnlyCollection<Facility>> GetListAsync(CancellationToken token = default);
}
