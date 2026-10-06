using AirWeb.AppServices.Compliance.FacilitySearch;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.Search;

namespace AirWeb.WebApp.Pages.DownloadSearch;

[Authorize(Policy = nameof(Policies.Staff))]
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
public abstract class DownloadBase<TSearchDto, TResultDto, TExportDto>(
    ISearchService<TSearchDto, TResultDto, TExportDto> searchService) : PageModel
#pragma warning restore S2436
    where TSearchDto : ISearchDto<TSearchDto>, IDeleteStatus
    where TResultDto : class, IFacilitySearchResult
    where TExportDto : IFacilitySearchResult
{
    public IRouteValues Spec { get; private set; } = null!;
    public int ResultsCount { get; private set; }

    protected async Task<IActionResult> DoGetAsync(TSearchDto? spec, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        ResultsCount = await searchService.CountAsync(spec, token);
        Spec = spec;
        return Page();
    }

    protected async Task<IActionResult> DoGetDownloadAsync(TSearchDto? spec, string name, CancellationToken token)
    {
        if (spec is null) return BadRequest();
        var exportList = await searchService.ExportAsync(spec, token);
        return this.ExcelFile(exportList, $"{name} Search Results", $"{name}_Search");
    }
}
