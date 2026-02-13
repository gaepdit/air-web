using AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.Staff))]
public class ComplianceIndexModel(
    IComplianceWorkSearchService searchService,
    IStaffService staff,
    IOfficeService offices) : PageModel
{
    public ComplianceWorkSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool UserCanViewDeletedRecords { get; private set; }
    public IPaginatedResult<ComplianceWorkSearchResultDto> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    // Select lists
    public SelectList StaffSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;

    public async Task OnGetAsync(CancellationToken token = default)
    {
        Spec = new ComplianceWorkSearchDto();
        UserCanViewDeletedRecords = User.CanManageDeletions();
        await PopulateSelectListsAsync(token);
    }

    public async Task OnGetSearchAsync(ComplianceWorkSearchDto spec, [FromQuery] int p = 1,
        CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        UserCanViewDeletedRecords = User.CanManageDeletions();
        if (!UserCanViewDeletedRecords) Spec = Spec with { DeleteStatus = null };

        await PopulateSelectListsAsync(token);

        if (!ModelState.IsValid) return;

        var paging = new PaginatedRequest(pageNumber: p, SearchDefaults.PageSize, sorting: Spec.Sort.GetDescription());
        SearchResults = await searchService.SearchAsync(Spec, paging, token: token);
        ShowResults = true;
    }

    private async Task PopulateSelectListsAsync(CancellationToken token = default)
    {
        StaffSelectList = (await staff.GetUsersAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true, token: token)).ToSelectList();
    }
}
