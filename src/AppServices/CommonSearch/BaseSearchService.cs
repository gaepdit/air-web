using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Users;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.CommonSearch;

public abstract class BaseSearchService<TEntity, TSearchDto, TResultDto, TExportDto>(
    IRepository<TEntity, int> repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    where TEntity : class, IEntity<int>
    where TSearchDto : class, ISearchDto<TSearchDto>, IDeleteStatus
    where TResultDto : class, ISearchResult
    where TExportDto : class, ISearchResult
{
    protected async Task<IPaginatedResult<TResultDto>> SearchAsync(
        TSearchDto spec,
        PaginatedRequest paging,
        bool loadFacilities,
        Func<TSearchDto, Expression<Func<TEntity, bool>>> filterPredicate,
        CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = filterPredicate(spec.TrimAll());

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

    protected async Task<int> CountAsync(
        TSearchDto spec,
        Func<TSearchDto, Expression<Func<TEntity, bool>>> filterPredicate,
        CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = filterPredicate(spec.TrimAll());
        return await repository.CountAsync(expression, token).ConfigureAwait(false);
    }

    protected async Task<IEnumerable<TExportDto>> ExportAsync(
        TSearchDto spec,
        Func<TSearchDto, Expression<Func<TEntity, bool>>> filterPredicate,
        CancellationToken token = default)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = filterPredicate(spec.TrimAll());

        var list = mapper.Map<IReadOnlyCollection<TExportDto>>(await repository
            .GetListAsync(expression, spec.Sorting, token: token)
            .ConfigureAwait(false));

        foreach (var result in list)
            result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

        return list;
    }

    // Common
    private async Task CheckDeleteStatusAuth(TSearchDto spec)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.ComplianceManager).ConfigureAwait(false))
            spec.DeleteStatus = null;
    }
}
