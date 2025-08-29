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

    public Task<List<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        bool forceRefresh = false) =>
        Task.FromResult(Items.Where(report => report.Facility?.Id == facilityId)
            .Select(report => new SourceTestSummary(report))
            .ToList());
}
