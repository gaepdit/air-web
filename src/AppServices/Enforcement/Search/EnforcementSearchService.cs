using AirWeb.AppServices.Compliance.Search;
using System.Linq.Expressions;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Users;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Search
{
    public abstract class EnforcementSearchService<TEntity>(
        IRepository<TEntity, int> repository,
        IFacilityService facilityService,
        IMapper mapper,
        IUserService userService,
        IAuthorizationService authorization)
        where TEntity : class, IEntity<int>, IComplianceEntity
    {
        protected async Task<IPaginatedResult<TResultDto>> GetSearchResultsAsync<TResultDto>(PaginatedRequest paging,
        Expression<Func<TEntity, bool>> expression, bool loadFacilities, CancellationToken token = default)
        where TResultDto : class, IStandardSearchResult
        {
            var count = await repository.CountAsync(expression, token).ConfigureAwait(false);

            var list = count > 0
                ? mapper.Map<IReadOnlyCollection<TResultDto>>(
                    await repository.GetPagedListAsync(expression, paging, token).ConfigureAwait(false))
                : [];

            if (!loadFacilities) return new PaginatedResult<TResultDto>(list, count, paging);

            foreach (var result in list)
                result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

            return new PaginatedResult<TResultDto>(list, count, paging);
        }
        public async Task<IPaginatedResult<EnforcementSearchResultDto>> SearchAsync(EnforcementSearchDto spec,
            PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
        {
            List<EnforcementSearchResultDto> mockData = [new EnforcementSearchResultDto { FacilityId = "00100010", DiscoveryDate = new DateOnly(2025, 4, 22), CaseFileStatus = Domain.EnforcementEntities.CaseFiles.CaseFileStatus.Open }];
            return new PaginatedResult<EnforcementSearchResultDto>(mockData , 1, new PaginatedRequest(1, 1) );
        }
        public async Task<int> CountAsync(EnforcementSearchDto spec, CancellationToken token)
        {
            return 1;
        }
    }
}
