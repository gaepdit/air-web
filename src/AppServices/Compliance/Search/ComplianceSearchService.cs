using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

public abstract class ComplianceSearchService<TEntity>(
    IRepository<TEntity, int> repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    where TEntity : class, IEntity<int>
{
    // Common
    protected async Task CheckDeleteStatusAuth<TSearchDto>(TSearchDto spec)
        where TSearchDto : ISearchDto
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.ComplianceManager).ConfigureAwait(false))
            spec.DeleteStatus = null;
    }

    protected async Task<IPaginatedResult<TResultDto>> GetSearchResultsAsync<TResultDto>(PaginatedRequest paging,
        Expression<Func<TEntity, bool>> expression, bool loadFacilities, CancellationToken token = default)
        where TResultDto : class, ISearchResult
    {
        var count = await repository.CountAsync(expression, token).ConfigureAwait(false);

        var list = count > 0
            ? mapper.Map<IReadOnlyCollection<TResultDto>>(
                await repository.GetPagedListAsync(expression, paging, token: token).ConfigureAwait(false))
            : [];

        if (!loadFacilities) return new PaginatedResult<TResultDto>(list, count, paging);

        foreach (var result in list)
            result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

        return new PaginatedResult<TResultDto>(list, count, paging);
    }

    protected async Task<IEnumerable<TResultDto>> GetExportResultsAsync<TResultDto>(
        Expression<Func<TEntity, bool>> expression, string sorting, CancellationToken token = default)
        where TResultDto : class, ISearchResult
    {
        var list = mapper.Map<IReadOnlyCollection<TResultDto>>(await repository
            .GetListAsync(expression, sorting, token: token)
            .ConfigureAwait(false));

        foreach (var result in list)
        {
            result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);
        }

        return list;
    }
}
