using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.DataExport;

public sealed class SearchResultsExportService(
    IWorkEntryRepository workEntryRepository,
    IUserService userService,
    IAuthorizationService authorization)
    : ISearchResultsExportService
{
    public async Task<int> CountAsync(WorkEntrySearchDto spec, CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.ComplianceManager).ConfigureAwait(false))
            spec.DeleteStatus = null;

        return await workEntryRepository.CountAsync(WorkEntryFilters.SearchPredicate(spec), token)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(WorkEntrySearchDto spec,
        CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.ComplianceManager).ConfigureAwait(false))
            spec.DeleteStatus = null;

        return (await workEntryRepository.GetListAsync(WorkEntryFilters.SearchPredicate(spec), token)
                .ConfigureAwait(false))
            .Select(entry => new SearchResultsExportDto(entry)).ToList();
    }

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => workEntryRepository.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await workEntryRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
