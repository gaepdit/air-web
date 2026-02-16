using AirWeb.AppServices.Compliance.FacilitySearch;
using AirWeb.AppServices.Core.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Compliance.Search;

#pragma warning disable S2436 // Types and methods should not have too many generic parameters
public interface ISearchService<in TSearchDto, TResultDto, TExportDto> : IDisposable, IAsyncDisposable
#pragma warning restore S2436
    where TSearchDto : ISearchDto<TSearchDto>
    where TResultDto : class, IFacilitySearchResult
    where TExportDto : IFacilitySearchResult
{
    Task<IPaginatedResult<TResultDto>> SearchAsync(TSearchDto spec, PaginatedRequest paging,
        bool loadFacilities = true, CancellationToken token = default);

    Task<int> CountAsync(TSearchDto spec, CancellationToken token = default);

    Task<IEnumerable<TExportDto>> ExportAsync(TSearchDto spec, CancellationToken token = default);
}
