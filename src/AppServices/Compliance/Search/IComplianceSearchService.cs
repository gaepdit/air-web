using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Compliance.Search;

public interface IComplianceSearchService : IDisposable, IAsyncDisposable
{
    // Work entries
    Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchWorkEntriesAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, CancellationToken token = default);

    Task<int> CountWorkEntriesAsync(WorkEntrySearchDto spec, CancellationToken token);

    Task<IReadOnlyList<WorkEntryExportDto>> ExportWorkEntriesAsync(WorkEntrySearchDto spec,
        CancellationToken token);

    // FCEs
    Task<IPaginatedResult<FceSearchResultDto>> SearchFcesAsync(FceSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default);

    Task<int> CountFcesAsync(FceSearchDto spec, CancellationToken token);

    Task<IReadOnlyList<FceExportDto>> ExportFcesAsync(FceSearchDto spec,
        CancellationToken token);
}
