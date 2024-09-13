namespace AirWeb.Domain.ExternalEntities.Facilities;

public interface IFacilityRepository : IDisposable, IAsyncDisposable
{
    Task<bool> FacilityExistsAsync(FacilityId id, CancellationToken token = default);
    Task<Facility?> FindFacilityAsync(FacilityId? id, CancellationToken token = default);
    Task<Facility> GetFacilityAsync(FacilityId id, CancellationToken token = default);
    Task<IReadOnlyCollection<Facility>> GetListAsync(CancellationToken token = default);
}
