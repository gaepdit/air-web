namespace AirWeb.Domain.Entities.Facilities;

public interface IFacilityRepository : IDisposable, IAsyncDisposable
{
    Task<bool> FacilityExistsAsync(FacilityId id, CancellationToken token = default);
    Task<bool> FacilityExistsAsync(string id, CancellationToken token = default);
    Task<Facility?> FindFacilityAsync(FacilityId id, CancellationToken token = default);
    Task<Facility?> FindFacilityAsync(string id, CancellationToken token = default);
    Task<Facility> GetFacilityAsync(FacilityId id, CancellationToken token = default);
    Task<Facility> GetFacilityAsync(string id, CancellationToken token = default);
}
