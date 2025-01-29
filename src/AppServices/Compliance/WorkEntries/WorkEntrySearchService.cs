using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AutoMapper;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public sealed class WorkEntrySearchService(
    IWorkEntrySearchRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    : ComplianceSearchService<WorkEntry>(repository, facilityService, mapper, userService, authorization),
        IWorkEntrySearchService
{
    public async Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = WorkEntryFilters.SearchPredicate(spec.TrimAll());
        return await GetSearchResultsAsync<WorkEntrySearchResultDto>(paging, expression, loadFacilities, token)
            .ConfigureAwait(false);
    }

    public async Task<int> CountAsync(WorkEntrySearchDto spec, CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = WorkEntryFilters.SearchPredicate(spec.TrimAll());
        return await repository.CountRecordsAsync(expression, token).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<WorkEntryExportDto>> ExportAsync(WorkEntrySearchDto spec, CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = WorkEntryFilters.SearchPredicate(spec.TrimAll());
        return await GetExportResultsAsync<WorkEntryExportDto>(expression, spec.Sort.GetDescription(), token)
            .ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
