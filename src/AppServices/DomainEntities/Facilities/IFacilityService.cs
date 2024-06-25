using AirWeb.Domain.ExternalEntities.Facilities;

namespace AirWeb.AppServices.DomainEntities.Facilities;

public interface IFacilityService : IDisposable, IAsyncDisposable
{
    Task<FacilityViewDto?> FindAsync(FacilityId id, CancellationToken token = default);
}
