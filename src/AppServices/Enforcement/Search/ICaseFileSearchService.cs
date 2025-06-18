using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Enforcement.Search
{
    public interface ICaseFileSearchService
    {
        Task<IPaginatedResult<CaseFileSearchResultDto>> SearchAsync(CaseFileSearchDto spec, PaginatedRequest paging,
            bool loadFacilities = true, CancellationToken token = default);

        Task<int> CountAsync(CaseFileSearchDto spec, CancellationToken token);
    }
}
