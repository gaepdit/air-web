using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Enforcement.Search
{
    public class CaseFileSearchService(
        ICaseFileRepository repository,
        IFacilityService facilityService,
        IMapper mapper) : ICaseFileSearchService
    {
        public async Task<int> CountAsync(CaseFileSearchDto spec, CancellationToken token)
        {
            return 1;
        }

        public async Task<IPaginatedResult<CaseFileSearchResultDto>> SearchAsync(CaseFileSearchDto spec,
            PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
        {
            var expression = CaseFileFilters.SearchPredicate(spec.TrimAll());
            var count = await repository.CountAsync(expression, token).ConfigureAwait(false);

            var list = count > 0
                ? mapper.Map<IReadOnlyCollection<CaseFileSearchResultDto>>(
                    await repository.GetPagedListAsync(expression, paging, token: token).ConfigureAwait(false))
                : [];

            if (!loadFacilities) return new PaginatedResult<CaseFileSearchResultDto>(list, count, paging);

            foreach (var result in list)
                result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

            return new PaginatedResult<CaseFileSearchResultDto>(list, count, paging);
        }
    }
}
