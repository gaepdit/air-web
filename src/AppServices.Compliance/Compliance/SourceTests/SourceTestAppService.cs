using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace AirWeb.AppServices.Compliance.Compliance.SourceTests;

public interface ISourceTestAppService
{
    Task<bool> SourceTestExistsAsync(int referenceNumber);

    Task<IPaginatedResult<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        PaginatedRequest paging);

    Task<IPaginatedResult<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(string? assignmentUser,
        Guid? assignmentOffice, PaginatedRequest paging);

    Task<IReadOnlyCollection<SourceTestAssignment>> GetOpenSourceTestAssignmentsAsync();
}

public class SourceTestAppService(ISourceTestService sourceTestService) : ISourceTestAppService
{
    public Task<bool> SourceTestExistsAsync(int referenceNumber) =>
        sourceTestService.SourceTestExistsAsync(referenceNumber);

    public async Task<IPaginatedResult<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId,
        PaginatedRequest paging)
    {
        var tests = await sourceTestService.GetSourceTestsForFacilityAsync(facilityId)
            .ConfigureAwait(false);

        return new PaginatedResult<SourceTestSummary>(tests.Skip(paging.Skip).Take(paging.Take),
            tests.Count, paging);
    }

    public async Task<IPaginatedResult<SourceTestSummary>> GetOpenSourceTestsForComplianceAsync(string? assignmentUser,
        Guid? assignmentOffice, PaginatedRequest paging)
    {
        var tests = await sourceTestService
            .GetOpenSourceTestsForComplianceAsync(assignmentUser, assignmentOffice, paging.Skip, paging.Take)
            .ConfigureAwait(false);

        return new PaginatedResult<SourceTestSummary>(tests.Item1, tests.Item2, paging);
    }

    public async Task<IReadOnlyCollection<SourceTestAssignment>> GetOpenSourceTestAssignmentsAsync() =>
        await sourceTestService.GetOpenSourceTestAssignmentsAsync().ConfigureAwait(false);
}
