using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public interface ISourceTestService
{
    Task<BaseSourceTestReport?> FindAsync(FacilityId facilityId, int referenceNumber);
    Task<List<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId);
}
