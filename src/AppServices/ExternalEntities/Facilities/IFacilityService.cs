using AirWeb.Domain.ExternalEntities.Facilities;

namespace AirWeb.AppServices.ExternalEntities.Facilities;

public interface IFacilityService : IDisposable, IAsyncDisposable
{
    Task<FacilityViewDto?> FindAsync(FacilityId id, CancellationToken token = default);
    Task<FacilityViewDto?> FindAsync(string? id, CancellationToken token = default);
    Task<IReadOnlyCollection<FacilityViewDto>> GetListAsync(CancellationToken token = default);
}
