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
    IComplianceSearchRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization) : IComplianceSearchService
{
    // Work Entries
    public async Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchWorkEntriesAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = WorkEntryFilters.SearchPredicate(spec.TrimAll());
        return await GetSearchResultsAsync<WorkEntrySearchResultDto, WorkEntry>(paging, expression, loadFacilities,
            token).ConfigureAwait(false);
    }

    public async Task<int> CountWorkEntriesAsync(WorkEntrySearchDto spec, CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = WorkEntryFilters.SearchPredicate(spec.TrimAll());
        return await repository.CountRecordsAsync(expression, token).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<WorkEntryExportDto>> ExportWorkEntriesAsync(WorkEntrySearchDto spec,
        CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = WorkEntryFilters.SearchPredicate(spec.TrimAll());
        return await GetExportResultsAsync<WorkEntryExportDto, WorkEntry>(expression, token).ConfigureAwait(false);
    }

    // FCEs
    public async Task<IPaginatedResult<FceSearchResultDto>> SearchFcesAsync(FceSearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = FceFilters.SearchPredicate(spec.TrimAll());
        return await GetSearchResultsAsync<FceSearchResultDto, Fce>(paging, expression, loadFacilities, token)
            .ConfigureAwait(false);
    }

    public async Task<int> CountFcesAsync(FceSearchDto spec, CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = FceFilters.SearchPredicate(spec.TrimAll());
        return await repository.CountRecordsAsync(expression, token).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<FceExportDto>> ExportFcesAsync(FceSearchDto spec, CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = FceFilters.SearchPredicate(spec.TrimAll());
        return await GetExportResultsAsync<FceExportDto, Fce>(expression, token).ConfigureAwait(false);
    }

    // Common
    private async Task CheckDeleteStatusAuth<TSearchDto>(TSearchDto spec)
        where TSearchDto : IComplianceSearchDto
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.ComplianceManager).ConfigureAwait(false))
            spec.DeleteStatus = null;
    }

    private async Task<IPaginatedResult<TResultDto>> GetSearchResultsAsync<TResultDto, TEntity>(PaginatedRequest paging,
        Expression<Func<TEntity, bool>> expression, bool loadFacilities, CancellationToken token = default)
        where TResultDto : class, IStandardSearchResult
        where TEntity : class, IEntity<int>, IComplianceEntity
    {
        var count = await repository.CountRecordsAsync(expression, token).ConfigureAwait(false);
        var collection = count > 0
            ? mapper.Map<IEnumerable<TResultDto>>(
                await repository.GetFilteredRecordsAsync(expression, paging, token).ConfigureAwait(false)).ToList()
            : [];

        if (!loadFacilities) return new PaginatedResult<TResultDto>(collection, count, paging);

        foreach (var result in collection)
            result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

        return new PaginatedResult<TResultDto>(collection, count, paging);
    }

    private async Task<IReadOnlyList<TResultDto>> GetExportResultsAsync<TResultDto, TEntity>(
        Expression<Func<TEntity, bool>> expression, CancellationToken token = default)
        where TResultDto : class, IStandardSearchResult
        where TEntity : class, IEntity<int>, IComplianceEntity
    {
        var collection = mapper.Map<IEnumerable<TResultDto>>(
            await repository.GetFilteredRecordsAsync(expression, token).ConfigureAwait(false)).ToList();

        foreach (var result in collection)
            result.FacilityName ??= await facilityService.GetNameAsync(result.FacilityId).ConfigureAwait(false);

        return collection;
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
