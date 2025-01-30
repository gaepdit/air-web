using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Compliance.Search;

// FUTURE: Is this reusable for Enforcement search?
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
public interface IComplianceSearchService<in TSearchDto, TSearchResultDto, TExportDto> : IDisposable, IAsyncDisposable
    where TSearchResultDto : class
#pragma warning restore S2436
{
    Task<IPaginatedResult<TSearchResultDto>> SearchAsync(TSearchDto spec, PaginatedRequest paging,
        bool loadFacilities = true, CancellationToken token = default);

    Task<int> CountAsync(TSearchDto spec, CancellationToken token);

    Task<IEnumerable<TExportDto>> ExportAsync(TSearchDto spec, CancellationToken token);
}
