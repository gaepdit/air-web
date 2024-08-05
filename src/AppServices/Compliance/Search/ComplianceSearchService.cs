using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Search;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

public sealed class ComplianceSearchService(
    ISearchRepository searchRepository,
    IFacilityRepository facilityRepository,
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization) : IComplianceSearchService
{
    public async Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchWorkEntriesAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, CancellationToken token = default)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = WorkEntryFilters.SearchPredicate(spec.TrimAll());
        return await SearchAsync<WorkEntrySearchResultDto, WorkEntry>(paging, expression, token).ConfigureAwait(false);
    }

    public async Task<IPaginatedResult<FceSearchResultDto>> SearchFcesAsync(FceSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = FceFilters.SearchPredicate(spec.TrimAll());
        return await SearchAsync<FceSearchResultDto, Fce>(paging, expression, token).ConfigureAwait(false);
    }

    private async Task CheckDeleteStatusAuth<TSearchDto>(TSearchDto spec)
        where TSearchDto : IDeleteStatusSearch
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeleteStatus = null;
    }

    private async Task<IPaginatedResult<TResultDto>> SearchAsync<TResultDto, TEntity>(PaginatedRequest paging,
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TResultDto : class, IFacilityInfo
        where TEntity : class, IEntity<int>
    {
        var count = await searchRepository.CountRecordsAsync(expression, token).ConfigureAwait(false);
        var collection = count > 0
            ? mapper.Map<IEnumerable<TResultDto>>(await searchRepository
                .GetFilteredRecordsAsync(expression, paging, token).ConfigureAwait(false))
            : [];

        var items = collection as TResultDto[] ?? collection.ToArray();

        foreach (var dto in items)
        {
            dto.FacilityName = await facilityRepository.GetFacilityNameAsync((FacilityId)dto.FacilityId, token)
                .ConfigureAwait(false);
        }

        return new PaginatedResult<TResultDto>(items, count, paging);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        searchRepository.Dispose();
        facilityRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await searchRepository.DisposeAsync().ConfigureAwait(false);
        await facilityRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
