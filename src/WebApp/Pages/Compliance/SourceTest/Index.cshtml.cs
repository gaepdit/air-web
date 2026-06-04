using AirWeb.AppServices.Compliance.Compliance.SourceTests;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.SourceTests.Models;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Compliance.SourceTest;

[Authorize(Policy = nameof(Policies.Staff))]
public class SourceTestIndexModel(ISourceTestAppService testsService, IStaffService staff)
    : PageModel
{
    [BindProperty, Required(ErrorMessage = "Enter a Reference ID.")]
    public string? FindId { get; set; }

    [BindProperty(SupportsGet = true), FromQuery, Display(Name = "Staff")]
    // Guid as string
    public string? Staff { get; set; }

    [BindProperty(SupportsGet = true), FromQuery, Display(Name = "Office")]
    public Guid? Office { get; set; }

    public string? UserId { get; private set; }

    public IPaginatedResult<SourceTestSummary> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(RouteValues, SearchResults);

    public IDictionary<string, string?> RouteValues => new Dictionary<string, string?>
        { { nameof(Office), Office.ToString() }, { nameof(Staff), Staff } };

    // Select lists
    public SelectList StaffSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync([FromQuery] int p = 1) =>
        await PageWithSearchResultsAsync(p);

    public async Task<IActionResult> OnPostAsync([FromQuery] int p = 1)
    {
        if (!ModelState.IsValid) return await PageWithSearchResultsAsync(p);

        if (!int.TryParse(FindId, out var referenceNumber))
            ModelState.AddModelError(nameof(FindId), "Reference Number must be a number.");
        else if (!await testsService.SourceTestExistsAsync(referenceNumber))
            ModelState.AddModelError(nameof(FindId), "The Reference Number entered does not exist.");

        return ModelState.IsValid
            ? RedirectToPage("Details", routeValues: new { referenceNumber })
            : await PageWithSearchResultsAsync(p);
    }

    private async Task<IActionResult> PageWithSearchResultsAsync(int p)
    {
        var paging = new PaginatedRequest(pageNumber: p, SearchDefaults.PageSize, sorting: "default");

        SearchResults = await testsService.GetOpenSourceTestsForComplianceAsync(assignmentUser: Staff,
            assignmentOffice: Office, paging: paging);

        UserId = (await staff.GetCurrentUserAsync()).Id;

        await PopulateSelectListsAsync();
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        var assignments = await testsService.GetOpenSourceTestAssignmentsAsync().ConfigureAwait(false);

        StaffSelectList = assignments
            .Select(a => new ListItem<string>(a.UserId, a.SortableNameWithOffice))
            .OrderBy(e => e.Name)
            .ToSelectList();

        OfficesSelectList = assignments
            .Where(a => a is { OfficeId: not null, OfficeName: not null })
            .Select(a => new ListItem(a.OfficeId!.Value, a.OfficeName!))
            .Distinct()
            .OrderBy(e => e.Name)
            .ToSelectList();
    }
}
