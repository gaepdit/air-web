using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Core.AuthenticationServices;

namespace AirWeb.WebApp.Pages.DownloadSearch;

[Authorize(Policy = nameof(Policies.Staff))]
public class FceDownload(IFceSearchService searchService)
    : DownloadBase<FceSearchDto, FceSearchResultDto, FceExportDto>(searchService)
{
    public async Task<IActionResult> OnGetAsync(FceSearchDto? spec, CancellationToken token) =>
        await DoGetAsync(spec, token);

    public async Task<IActionResult> OnGetDownloadAsync(FceSearchDto? spec, CancellationToken token) =>
        await DoGetDownloadAsync(spec, "FCE", token);
}
