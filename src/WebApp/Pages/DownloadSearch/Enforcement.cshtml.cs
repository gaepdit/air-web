using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.DownloadSearch;

[Authorize(Policy = nameof(Policies.Staff))]
public class EnforcementDownload(ICaseFileSearchService searchService)
    : DownloadBase<CaseFileSearchDto, CaseFileSearchResultDto, CaseFileExportDto>(searchService)
{
    public async Task<IActionResult> OnGetAsync(CaseFileSearchDto? spec, CancellationToken token) =>
        await DoGetAsync(spec, token);

    public async Task<IActionResult> OnGetDownloadAsync(CaseFileSearchDto? spec, CancellationToken token) =>
        await DoGetDownloadAsync(spec, "Enforcement", token);
}
