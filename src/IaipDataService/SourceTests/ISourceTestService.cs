using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public interface ISourceTestService
{
    Task<BaseSourceTestReport?> FindAsync(int referenceNumber);
    Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber, bool forceRefresh = false);

    Task<IReadOnlyCollection<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        bool forceRefresh = false);

    Task<(IReadOnlyCollection<SourceTestSummary>, int)> GetOpenSourceTestsForComplianceAsync(string? assignmentEmail,
        int skip, int take);

    Task UpdateSourceTestAsync(int referenceNumber, string assignmentEmail, DateOnly? reviewDate);
}
