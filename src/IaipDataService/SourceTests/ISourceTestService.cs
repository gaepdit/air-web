using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public interface ISourceTestService
{
    Task<BaseSourceTestReport?> FindAsync(int referenceNumber);

    Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber);

    Task<bool> SourceTestExistsAsync(int referenceNumber);

    Task<IReadOnlyCollection<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId);

    Task<(IReadOnlyCollection<SourceTestSummary>, int)> GetOpenSourceTestsForComplianceAsync(string? assignmentUser,
        Guid? assignmentOffice, int skip, int take);

    Task UpdateSourceTestAsync(int referenceNumber, string assignmentEmail, DateOnly? reviewDate);
}
