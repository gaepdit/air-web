using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Search;
using AutoMapper;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

public sealed class ComplianceSearchService(
    IComplianceSearchRepository complianceSearchRepository,
    IFacilityService facilityService,
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
        where TSearchDto : IComplianceSearchDto
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.ComplianceManager).ConfigureAwait(false))
            spec.DeleteStatus = null;
    }

    private async Task<IPaginatedResult<TResultDto>> SearchAsync<TResultDto, TEntity>(PaginatedRequest paging,
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TResultDto : class, IStandardSearchResult
        where TEntity : class, IEntity<int>, IComplianceEntity
    {
        var count = await complianceSearchRepository.CountRecordsAsync(expression, token).ConfigureAwait(false);
        var collection = count > 0
            ? mapper.Map<IEnumerable<TResultDto>>(await complianceSearchRepository
                .GetFilteredRecordsAsync(expression, paging, token).ConfigureAwait(false))
            : [];
        return new PaginatedResult<TResultDto>(collection, count, paging);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        complianceSearchRepository.Dispose();
        facilityService.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await complianceSearchRepository.DisposeAsync().ConfigureAwait(false);
        await facilityService.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
