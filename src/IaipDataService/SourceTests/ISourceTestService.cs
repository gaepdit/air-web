using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public interface ISourceTestService
{
    Task<BaseSourceTestReport?> FindAsync(int referenceNumber);
    Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber);
    Task<List<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId);
}
