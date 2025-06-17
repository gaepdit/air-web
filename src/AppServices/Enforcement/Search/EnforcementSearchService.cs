using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement.Search
{
    public class EnforcementSearchService(
        ICaseFileRepository repository,
        IFacilityService facilityService,
        IMapper mapper) : IEnforcementSearchService
    {
        public async Task<int> CountAsync(EnforcementSearchDto spec, CancellationToken token)
        {
            return 1;
        }

        public async Task<IPaginatedResult<EnforcementSearchResultDto>> SearchAsync(EnforcementSearchDto spec,
            PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
        {
            var expression = EnforcementFilters.SearchPredicate(spec.TrimAll());
            var count = await repository.CountAsync(expression, token).ConfigureAwait(false);

            var list = count > 0
                ? mapper.Map<IReadOnlyCollection<EnforcementSearchResultDto>>(
                    await repository.GetPagedListAsync(expression, paging, token: token).ConfigureAwait(false))
                : [];

            if (!loadFacilities) return new PaginatedResult<EnforcementSearchResultDto>(list, count, paging);

            foreach (var result in list)
                result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

            return new PaginatedResult<EnforcementSearchResultDto>(list, count, paging);
        }
    }
}
