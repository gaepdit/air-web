using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Core.AuthorizationServices;

namespace AirWeb.WebApp.Pages.DownloadSearch;

[Authorize(Policy = nameof(Policies.Staff))]
public class ComplianceWorkDownload(IComplianceWorkSearchService searchService)
    : DownloadBase<ComplianceWorkSearchDto, ComplianceWorkSearchResultDto, ComplianceWorkExportDto>(searchService)
{
    public async Task<IActionResult> OnGetAsync(ComplianceWorkSearchDto? spec, CancellationToken token) =>
        await DoGetAsync(spec, token);

    public async Task<IActionResult> OnGetDownloadAsync(ComplianceWorkSearchDto? spec, CancellationToken token) =>
        await DoGetDownloadAsync(spec, "ComplianceWork", token);
}
