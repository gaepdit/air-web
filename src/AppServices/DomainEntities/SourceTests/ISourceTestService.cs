using AirWeb.Domain.ExternalEntities.Facilities;

namespace AirWeb.AppServices.DomainEntities.SourceTests;

public interface ISourceTestService : IDisposable, IAsyncDisposable
{
    Task<SourceTestBasicViewDto?> FindAsync(FacilityId facilityId, int referenceNumber,
        CancellationToken token = default);
}
