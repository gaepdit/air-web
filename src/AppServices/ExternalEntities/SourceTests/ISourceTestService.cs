using IaipDataService.Facilities;

namespace AirWeb.AppServices.ExternalEntities.SourceTests;

public interface ISourceTestService : IDisposable, IAsyncDisposable
{
    Task<SourceTestBasicViewDto?> FindAsync(FacilityId facilityId, int referenceNumber,
        CancellationToken token = default);
}
