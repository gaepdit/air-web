using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.Staff))]
public class ComplianceIndexModel(
    IWorkEntrySearchService searchService,
    IStaffService staff,
    IOfficeService offices) : PageModel
{
    public WorkEntrySearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool UserCanViewDeletedRecords { get; private set; }
    public IPaginatedResult<WorkEntrySearchResultDto> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    // Select lists
    public SelectList StaffSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;

    public async Task OnGetAsync(CancellationToken token = default)
    {
        Spec = new WorkEntrySearchDto();
        UserCanViewDeletedRecords = User.CanManageDeletions();
        await PopulateSelectListsAsync(token);
    }

    public async Task OnGetSearchAsync(WorkEntrySearchDto spec, [FromQuery] int p = 1,
        CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        UserCanViewDeletedRecords = User.CanManageDeletions();
        if (!UserCanViewDeletedRecords) Spec = Spec with { DeleteStatus = null };

        var paging = new PaginatedRequest(pageNumber: p, GlobalConstants.PageSize, sorting: Spec.Sort.GetDescription());
        SearchResults = await searchService.SearchAsync(Spec, paging, token: token);

        ShowResults = true;
        await PopulateSelectListsAsync(token);
    }

    private async Task PopulateSelectListsAsync(CancellationToken token = default)
    {
        StaffSelectList = (await staff.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true, token: token)).ToSelectList();
    }
}
