using AirWeb.AppServices.Compliance.Compliance.SourceTests;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.SourceTests.Models;

namespace AirWeb.WebApp.Pages.Compliance.SourceTest;

[Authorize(Policy = nameof(Policies.Staff))]
public class SourceTestIndexModel(ISourceTestAppService sourceTestService) : PageModel
{
    public IPaginatedResult<SourceTestSummary> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(SearchResults);
    public int PageNumber { get; set; }

    [TempData]
    public bool RefreshIaipData { get; set; }

    public async Task<IActionResult> OnGetAsync([FromQuery] int p = 1, [FromQuery] bool showAll = false,
        [FromQuery] bool refresh = false, CancellationToken token = default)
    {
        if (refresh)
        {
            RefreshIaipData = true;
            return RedirectToPage(new { p, showAll });
        }

        PageNumber = p;
        var paging = new PaginatedRequest(pageNumber: p, SearchDefaults.PageSize, sorting: "default");
        var userEmail = showAll ? null : User.GetEmail();
        SearchResults = await sourceTestService.GetOpenSourceTestsForComplianceAsync(assignmentEmail: userEmail, paging,
            RefreshIaipData);

        return Page();
    }
}
