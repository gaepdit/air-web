using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.AppServices.Compliance.Compliance.SourceTests;

public interface ISourceTestAppService
{
    Task<IPaginatedResult<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        PaginatedRequest paging, bool forceRefresh = false);

    Task<IPaginatedResult<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(string? userEmail,
        PaginatedRequest paging, bool forceRefresh = false);
}

public class SourceTestAppService(ISourceTestService sourceTestService) : ISourceTestAppService
{
    public async Task<IPaginatedResult<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        PaginatedRequest paging, bool forceRefresh = false)
    {
        var tests = await sourceTestService.GetSourceTestsForFacilityAsync(facilityId, forceRefresh)
            .ConfigureAwait(false);

        return new PaginatedResult<SourceTestSummary>(tests.Skip(paging.Skip).Take(paging.Take),
            tests.Count, paging);
    }

    public async Task<IPaginatedResult<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(string? userEmail,
        PaginatedRequest paging, bool forceRefresh = false)
    {
        var allTests = await sourceTestService.GetOpenSourceTestsForComplianceAsync(forceRefresh)
            .ConfigureAwait(false);

        var filteredTests = userEmail is null
            ? allTests
            : allTests.Where(summary => summary.IaipComplianceAssignment == userEmail).ToList();

        return new PaginatedResult<SourceTestSummary>(filteredTests.Skip(paging.Skip).Take(paging.Take),
            filteredTests.Count, paging);
    }
}
