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

    public Task<(IReadOnlyCollection<SourceTestSummary>, int)> GetOpenSourceTestsForComplianceAsync(
        string? assignmentEmail, int skip, int take)
    {
        var allTests = Items
            .Where(report => report is { ReportClosed: true, IaipComplianceComplete: false })
            .Where(report => assignmentEmail is null || report.IaipComplianceAssignment == assignmentEmail)
            .OrderByDescending(report => report.DateTestReviewComplete)
            .ThenByDescending(report => report.ReferenceNumber).ToList();

        var testsPage = allTests.Skip(skip).Take(take).Select(report => new SourceTestSummary(report)).ToList();

        return Task.FromResult<(IReadOnlyCollection<SourceTestSummary>, int)>((testsPage, allTests.Count));
    }

    public Task UpdateSourceTestAsync(int referenceNumber, string assignmentEmail, DateOnly? reviewDate)
    {
        var report = Items.Single(report => report.ReferenceNumber == referenceNumber);
        report.IaipComplianceAssignment = assignmentEmail;
        report.IaipComplianceComplete = reviewDate != null;

        return Task.CompletedTask;
    }
}
