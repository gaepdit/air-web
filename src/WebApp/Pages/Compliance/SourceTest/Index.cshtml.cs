using AirWeb.AppServices.Compliance.Compliance.SourceTests;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.SourceTests.Models;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Compliance.SourceTest;

[Authorize(Policy = nameof(Policies.Staff))]
public class SourceTestIndexModel(ISourceTestAppService service, IStaffService staff, IOfficeService offices)
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

    public async Task<IActionResult> OnGetAsync([FromQuery] int p = 1, CancellationToken token = default) =>
        await PageWithSearchResultsAsync(p, token);

    public async Task<IActionResult> OnPostAsync([FromQuery] int p = 1, CancellationToken token = default)
    {
        if (!ModelState.IsValid) return await PageWithSearchResultsAsync(p, token);

        if (!int.TryParse(FindId, out var referenceNumber))
            ModelState.AddModelError(nameof(FindId), "Reference Number must be a number.");
        else if (!await service.SourceTestExistsAsync(referenceNumber))
            ModelState.AddModelError(nameof(FindId), "The Reference Number entered does not exist.");

        return ModelState.IsValid
            ? RedirectToPage("Details", routeValues: new { referenceNumber })
            : await PageWithSearchResultsAsync(p, token);
    }

    private async Task<IActionResult> PageWithSearchResultsAsync(int p, CancellationToken token)
    {
        var paging = new PaginatedRequest(pageNumber: p, SearchDefaults.PageSize, sorting: "default");

        SearchResults = await service.GetOpenSourceTestsForComplianceAsync(assignmentUser: Staff,
            assignmentOffice: Office, paging: paging);

        UserId = (await staff.GetCurrentUserAsync()).Id;

        await PopulateSelectListsAsync(token);
        return Page();
    }

    private async Task PopulateSelectListsAsync(CancellationToken token)
    {
        StaffSelectList = (await staff.GetStaffInRoleAsync(token, ComplianceRole.ComplianceStaffRole,
            ComplianceRole.ComplianceManagerRole).ConfigureAwait(false)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: false, token)).ToSelectList();
    }
}
