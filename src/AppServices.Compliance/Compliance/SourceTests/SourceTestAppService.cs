using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.AppServices.Compliance.Compliance.SourceTests;

public interface ISourceTestAppService
{
    Task<IPaginatedResult<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        PaginatedRequest paging, bool forceRefresh = false);

    Task<IPaginatedResult<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(string? assignmentEmail,
        PaginatedRequest paging);
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

    public async Task<IPaginatedResult<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(string? assignmentEmail,
        PaginatedRequest paging)
    {
        var tests = await sourceTestService
            .GetOpenSourceTestsForComplianceAsync(assignmentEmail, paging.Skip, paging.Take)
            .ConfigureAwait(false);

        return new PaginatedResult<SourceTestSummary>(tests.Item1, tests.Item2, paging);
    }
}
