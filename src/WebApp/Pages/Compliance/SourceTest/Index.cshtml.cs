using AirWeb.AppServices.Compliance.Compliance.SourceTests;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.SourceTests.Models;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Compliance.SourceTest;

[Authorize(Policy = nameof(Policies.Staff))]
public class SourceTestIndexModel(ISourceTestAppService service) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Enter a Reference ID.")]
    public string? FindId { get; set; }

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
        SearchResults = await service.GetOpenSourceTestsForComplianceAsync(assignmentEmail: userEmail, paging);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromQuery] int p = 1, [FromQuery] bool showAll = false,
        CancellationToken token = default)
    {
        if (!ModelState.IsValid) return Page();

        if (!int.TryParse(FindId, out var referenceNumber))
            ModelState.AddModelError(nameof(FindId), "Reference Number must be a number.");
        else if (!await service.SourceTestExistsAsync(referenceNumber))
            ModelState.AddModelError(nameof(FindId), "The Reference Number entered does not exist.");

        if (ModelState.IsValid) return RedirectToPage("Details", routeValues: new { referenceNumber });

        PageNumber = p;
        ShowAll = showAll;

        var paging = new PaginatedRequest(pageNumber: p, SearchDefaults.PageSize, sorting: "default");
        var userEmail = showAll ? null : User.GetEmail();
        SearchResults = await service.GetOpenSourceTestsForComplianceAsync(assignmentEmail: userEmail, paging);

        return Page();
    }

    public IActionResult OnGetSearch([FromQuery] int p = 1, [FromQuery] bool showAll = false,
        CancellationToken token = default) =>
        RedirectToPage("Index", pageHandler: null, routeValues: new { showAll, p }, fragment: "");
}
