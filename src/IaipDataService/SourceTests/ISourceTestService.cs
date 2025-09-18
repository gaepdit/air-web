using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public interface ISourceTestService
{
    Task<BaseSourceTestReport?> FindAsync(int referenceNumber);
    Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber, bool forceRefresh = false);

    Task<IReadOnlyCollection<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        bool forceRefresh = false);

    Task<IReadOnlyCollection<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(bool forceRefresh = false);
    Task UpdateSourceTest(int referenceNumber, string complianceAssignment, bool complianceComplete);
}
