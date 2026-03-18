using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.Permissions;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Domain.Compliance.EnforcementEntities.ViolationTypes;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.Staff))]
public class EnforcementIndexModel(
    ICaseFileSearchService searchService,
    ICaseFileService caseFileService,
    IStaffService staff,
    IOfficeService offices) : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Enter an Enforcement ID.")]
    [Display(Name = "Enforcement ID")]
    public string? FindId { get; set; }

    public CaseFileSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool UserCanViewDeletedRecords { get; private set; }
    public IPaginatedResult<CaseFileSearchResultDto> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    // Select lists
    public SelectList StaffSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;
    public SelectList ViolationTypeSelectList { get; set; } = null!;

    public async Task OnGetAsync(CancellationToken token = default)
    {
        UserCanViewDeletedRecords = User.CanManageCaseFileDeletions();
        await PopulateSelectListsAsync(token);
    }

    public async Task OnGetSearchAsync(CaseFileSearchDto spec, [FromQuery] int p = 1, CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        UserCanViewDeletedRecords = User.CanManageCaseFileDeletions();
        if (!UserCanViewDeletedRecords) Spec = Spec with { DeleteStatus = null };

        await PopulateSelectListsAsync(token);

        if (!ModelState.IsValid) return;

        var paging = new PaginatedRequest(pageNumber: p, SearchDefaults.PageSize, sorting: Spec.Sort.GetDescription());
        SearchResults = await searchService.SearchAsync(Spec, paging, token: token);
        ShowResults = true;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            UserCanViewDeletedRecords = User.CanManageCaseFileDeletions();
            await PopulateSelectListsAsync(token);
            return Page();
        }

        if (!int.TryParse(FindId, out var id))
            ModelState.AddModelError(nameof(FindId), "Enforcement ID must be a number.");
        else if (!await caseFileService.ExistsAsync(id, token: token))
            ModelState.AddModelError(nameof(FindId), "The Enforcement ID entered does not exist.");

        if (ModelState.IsValid) return RedirectToPage("Details", routeValues: new { id });

        UserCanViewDeletedRecords = User.CanManageCaseFileDeletions();
        await PopulateSelectListsAsync(token);
        return Page();
    }

    private async Task PopulateSelectListsAsync(CancellationToken token)
    {
        StaffSelectList = (await staff.GetUsersAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true, token: token)).ToSelectList();
        ViolationTypeSelectList = new SelectList(ViolationTypeData.GetAll(),
            nameof(ViolationType.Code), nameof(ViolationType.Display),
            null, nameof(ViolationType.Current));
    }
}
