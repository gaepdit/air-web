using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Cases;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.SBEAP;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class IndexModel(ICaseworkService cases) : PageModel
{
    public IPaginatedResult<CaseworkSearchResultDto> OpenCases { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var openCasesSpec = new CaseworkSearchDto { Status = CaseStatus.Open, Sort = CaseworkSortBy.OpenedDate };
        var paging = new PaginatedRequest(1, SearchDefaults.PageSize, openCasesSpec.Sort.GetDescription());
        OpenCases = await cases.SearchAsync(openCasesSpec, paging);

        return Page();
    }
}
