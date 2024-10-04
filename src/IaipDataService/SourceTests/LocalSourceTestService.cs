using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.SourceTests;

public class LocalSourceTestService : ISourceTestService
{
    private IReadOnlyCollection<BaseSourceTestReport> Items { get; } = SourceTestData.GetData.ToList();

    public Task<BaseSourceTestReport?> FindAsync(FacilityId facilityId, int referenceNumber,
        CancellationToken token = default)
    {
        var result = Items.SingleOrDefault(report =>
            report.ReferenceNumber == referenceNumber && report.Facility?.Id == facilityId);
        result?.ParseConfidentialParameters();
        return Task.FromResult(result);
    }
}
