using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.WorkEntries.Search;

namespace AirWeb.WebApp.Pages.DownloadSearch;

[Authorize(Policy = nameof(Policies.Staff))]
public class ComplianceWorkDownload(IWorkEntrySearchService searchService)
    : DownloadBase<WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>(searchService)
{
    public async Task<IActionResult> OnGetAsync(WorkEntrySearchDto? spec, CancellationToken token) =>
        await DoGetAsync(spec, token);

    public async Task<IActionResult> OnGetDownloadAsync(WorkEntrySearchDto? spec, CancellationToken token) =>
        await DoGetDownloadAsync(spec, "ComplianceWork", token);
}
