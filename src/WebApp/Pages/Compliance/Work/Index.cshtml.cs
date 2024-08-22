using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.Staff))]
public class ComplianceIndexModel(
    IComplianceSearchService searchService,
    IStaffService staff,
    IOfficeService offices,
    IAuthorizationService authorization)
    : PageModel
{
    public WorkEntrySearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public bool UserCanViewDeletedRecords { get; private set; }
    public IPaginatedResult<WorkEntrySearchResultDto> SearchResults { get; private set; } = default!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    // Select lists
    public SelectList StaffSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Spec = new WorkEntrySearchDto();
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.ComplianceManager);
        await PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(WorkEntrySearchDto spec, [FromQuery] int p = 1,
        CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.ComplianceManager);
        await PopulateSelectListsAsync();

        var paging = new PaginatedRequest(pageNumber: p, GlobalConstants.PageSize, sorting: Spec.Sort.GetDescription());
        SearchResults = await searchService.SearchWorkEntriesAsync(Spec, paging, token);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        StaffSelectList = (await staff.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
    }
}
