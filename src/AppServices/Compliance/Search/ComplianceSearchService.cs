using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities;
using AirWeb.Domain.ComplianceEntities.Search;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

public abstract class ComplianceSearchService<TEntity>(
    IComplianceSearchRepository<TEntity> repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    where TEntity : class, IEntity<int>, IComplianceEntity
{
    // Common
    protected async Task CheckDeleteStatusAuth<TSearchDto>(TSearchDto spec)
        where TSearchDto : IComplianceSearchDto
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.ComplianceManager).ConfigureAwait(false))
            spec.DeleteStatus = null;
    }

    protected async Task<IPaginatedResult<TResultDto>> GetSearchResultsAsync<TResultDto>(PaginatedRequest paging,
        Expression<Func<TEntity, bool>> expression, bool loadFacilities, CancellationToken token = default)
        where TResultDto : class, IStandardSearchResult
    {
        var count = await repository.CountRecordsAsync(expression, token).ConfigureAwait(false);

        var list = count > 0
            ? mapper.Map<IEnumerable<TResultDto>>(
                await repository.GetFilteredRecordsAsync(expression, paging, token).ConfigureAwait(false)).ToList()
            : [];

        if (!loadFacilities) return new PaginatedResult<TResultDto>(list, count, paging);

        foreach (var result in list)
            result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

        return new PaginatedResult<TResultDto>(list, count, paging);
    }

    protected async Task<IReadOnlyList<TResultDto>> GetExportResultsAsync<TResultDto>(
        Expression<Func<TEntity, bool>> expression, string sorting, CancellationToken token = default)
        where TResultDto : class, IStandardSearchResult
    {
        var list = mapper.Map<IEnumerable<TResultDto>>(
            await repository.GetFilteredRecordsAsync(expression, sorting, token).ConfigureAwait(false)).ToList();

        foreach (var result in list)
            result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

        return list;
    }
}
