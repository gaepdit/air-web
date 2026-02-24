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
    public PaginatedResultsDisplay ResultsDisplay => new(RouteValues, SearchResults);
    public int PageNumber { get; set; }
    public bool ShowAll { get; set; }

    public IDictionary<string, string?> RouteValues => new Dictionary<string, string?>
        { { nameof(ShowAll), ShowAll.ToString() } };

    public async Task<IActionResult> OnGetAsync([FromQuery] int p = 1, [FromQuery] bool showAll = false,
        CancellationToken token = default)
    {
        PageNumber = p;
        ShowAll = showAll;

        var paging = new PaginatedRequest(pageNumber: p, SearchDefaults.PageSize, sorting: "default");
        var userEmail = showAll ? null : User.GetEmail();
        SearchResults =
            await sourceTestService.GetOpenSourceTestsForComplianceAsync(assignmentEmail: userEmail, paging);

        return Page();
    }

    public IActionResult OnGetSearch([FromQuery] int p = 1, [FromQuery] bool showAll = false,
        CancellationToken token = default) =>
        RedirectToPage("Index", pageHandler: null, routeValues: new { showAll, p }, fragment: "");
}
