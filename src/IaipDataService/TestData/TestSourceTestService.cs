using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.TestData;

public class TestSourceTestService : ISourceTestService
{
    internal IReadOnlyCollection<BaseSourceTestReport> Items { get; } = SourceTestData.GetData.ToList();

    public Task<BaseSourceTestReport?> FindAsync(int referenceNumber)
    {
        var result = Items.SingleOrDefault(report => report.ReferenceNumber == referenceNumber);
        result?.ParseConfidentialParameters();
        return Task.FromResult(result);
    }

    public Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber, bool forceRefresh = false)
    {
        var result = Items.SingleOrDefault(report => report.ReferenceNumber == referenceNumber);
        return Task.FromResult(result is null ? null : new SourceTestSummary(result));
    }

    public Task<IReadOnlyCollection<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        bool forceRefresh = false) =>
        Task.FromResult<IReadOnlyCollection<SourceTestSummary>>(Items
            .Where(report => report.Facility?.Id == facilityId)
            .OrderByDescending(report => report.TestDates.StartDate)
            .ThenByDescending(report => report.ReferenceNumber)
            .Select(report => new SourceTestSummary(report)).ToList());

    public Task<IReadOnlyCollection<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(
        bool forceRefresh = false) =>
        Task.FromResult<IReadOnlyCollection<SourceTestSummary>>(Items
            .Where(report => report is { ReportClosed: true, IaipComplianceComplete: false })
            .OrderByDescending(report => report.DateTestReviewComplete)
            .ThenByDescending(report => report.ReferenceNumber)
            .Select(report => new SourceTestSummary(report)).ToList());

    public Task UpdateSourceTest(int referenceNumber, string complianceAssignment, bool complianceComplete)
    {
        throw new NotImplementedException();
    }
}
