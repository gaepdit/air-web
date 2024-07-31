using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.WorkEntries;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.DataExport;

public sealed class SearchResultsExportService(
    IWorkEntryRepository workEntryRepository,
    IUserService userService,
    IAuthorizationService authorization)
    : ISearchResultsExportService
{
    public async Task<int> CountAsync(ComplianceSearchDto spec, CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return await workEntryRepository.CountAsync(ComplianceSearchFilters.SearchPredicate(spec), token)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<SearchResultsExportDto>> ExportSearchResultsAsync(ComplianceSearchDto spec,
        CancellationToken token)
    {
        spec.TrimAll();
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeletedStatus = null;

        return (await workEntryRepository.GetListAsync(ComplianceSearchFilters.SearchPredicate(spec), token)
                .ConfigureAwait(false))
            .Select(entry => new SearchResultsExportDto(entry)).ToList();
    }

    #region IDisposable,  IAsyncDisposable

    void IDisposable.Dispose() => workEntryRepository.Dispose();
    async ValueTask IAsyncDisposable.DisposeAsync() => await workEntryRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
