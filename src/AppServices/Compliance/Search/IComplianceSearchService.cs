using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Compliance.Search;

public interface IComplianceSearchService : IDisposable, IAsyncDisposable
{
    Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchWorkEntriesAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, CancellationToken token = default);

    Task<IPaginatedResult<FceSearchResultDto>> SearchFcesAsync(FceSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default);
}
