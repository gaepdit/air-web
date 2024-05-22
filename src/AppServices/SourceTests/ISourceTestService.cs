using AirWeb.Domain.Entities.Facilities;

namespace AirWeb.AppServices.SourceTests;

public interface ISourceTestService : IDisposable, IAsyncDisposable
{
    Task<SourceTestBasicViewDto?> FindAsync(FacilityId facilityId, int referenceNumber,
        CancellationToken token = default);
}
