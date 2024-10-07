using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public class LocalSourceTestService : ISourceTestService
{
    private IReadOnlyCollection<BaseSourceTestReport> Items { get; } = SourceTestData.GetData.ToList();

    public Task<BaseSourceTestReport?> FindAsync(FacilityId facilityId, int referenceNumber)
    {
        var result = Items.SingleOrDefault(report =>
            report.ReferenceNumber == referenceNumber && report.Facility?.Id == facilityId);
        result?.ParseConfidentialParameters();
        return Task.FromResult(result);
    }

    public Task<List<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId) =>
        Task.FromResult(Items.Where(report => report.Facility?.Id == facilityId)
            .Select(report => new SourceTestSummary(report))
            .ToList());
}
