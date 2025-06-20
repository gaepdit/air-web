using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.CommonSearch;

public interface ISearchService<in TSearchDto, TResultDto, TExportDto> : IDisposable, IAsyncDisposable
    where TSearchDto : ISearchDto<TSearchDto>
    where TResultDto : class, ISearchResult
    where TExportDto : ISearchResult
{
    Task<IPaginatedResult<TResultDto>> SearchAsync(TSearchDto spec, PaginatedRequest paging,
        bool loadFacilities = true, CancellationToken token = default);

    Task<int> CountAsync(TSearchDto spec, CancellationToken token = default);

    Task<IEnumerable<TExportDto>> ExportAsync(TSearchDto spec, CancellationToken token = default);
}
