using AirWeb.AppServices.Enforcement.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.AppServices.Enforcement.Search
{
    public interface IEnforcementSearchService
    {
        Task<IPaginatedResult<EnforcementSearchResultDto>> SearchAsync(EnforcementSearchDto spec, PaginatedRequest paging,
    bool loadFacilities = true, CancellationToken token = default);

        Task<int> CountAsync(EnforcementSearchDto spec, CancellationToken token);
    }
}
