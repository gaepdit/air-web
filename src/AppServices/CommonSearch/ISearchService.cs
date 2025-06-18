using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.CommonSearch;

public interface ISearchService<in TSearchDto, TSearchResultDto, TExportDto> : IDisposable, IAsyncDisposable
    where TSearchDto : ISearchDto
    where TSearchResultDto : class, ISearchResult
    where TExportDto : ISearchResult
{
    Task<IPaginatedResult<TSearchResultDto>> SearchAsync(TSearchDto spec, PaginatedRequest paging,
        bool loadFacilities = true, CancellationToken token = default);

    Task<int> CountAsync(TSearchDto spec, CancellationToken token);

    Task<IEnumerable<TExportDto>> ExportAsync(TSearchDto spec, CancellationToken token);
}
