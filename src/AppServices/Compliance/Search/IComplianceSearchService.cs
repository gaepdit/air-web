using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Compliance.Search;

public interface IComplianceSearchService : IDisposable, IAsyncDisposable
{
    // Search
    Task<IPaginatedResult<ComplianceSearchResultDto>> SearchAsync(ComplianceSearchDto spec, PaginatedRequest paging,
        CancellationToken token = default);
}
