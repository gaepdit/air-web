using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public class IaipSourceTestService : ISourceTestService
{
    public Task<BaseSourceTestReport?> FindAsync(FacilityId facilityId, int referenceNumber,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
