using AirWeb.Domain.Entities.Facilities;

namespace AirWeb.AppServices.StackTests;

public interface IStackTestService : IDisposable, IAsyncDisposable
{
    Task<StackTestBasicViewDto?> FindAsync(FacilityId facilityId, int referenceNumber,
        CancellationToken token = default);
}
