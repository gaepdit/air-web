using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Compliance.Search;

// FUTURE: Is this reusable for Enforcement search?
public interface IComplianceSearchService<in TSearchDto, TSearchResultDto, TExportDto> : IDisposable, IAsyncDisposable
    where TSearchResultDto : class
{
    Task<IPaginatedResult<TSearchResultDto>> SearchAsync(TSearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default);

    Task<int> CountAsync(TSearchDto spec, CancellationToken token);

    Task<IReadOnlyList<TExportDto>> ExportAsync(TSearchDto spec,
        CancellationToken token);
}
