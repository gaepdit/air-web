using AirWeb.Domain.Entities.Facilities;

namespace AirWeb.AppServices.Facilities;

public interface IFacilityService : IDisposable, IAsyncDisposable
{
    Task<FacilityViewDto?> FindAsync(FacilityId id, CancellationToken token = default);
}
