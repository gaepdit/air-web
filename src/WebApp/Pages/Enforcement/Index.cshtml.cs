using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Defaults;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Enforcement;

[Authorize(Policy = nameof(Policies.Staff))]
public class EnforcementIndexModel(
    ICaseFileSearchService searchService,
    IStaffService staff,
    IOfficeService offices,
    IAuthorizationService authorization) : PageModel
{
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
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.EnforcementManager);
        await PopulateSelectListsAsync(token);
    }

    public async Task OnGetSearchAsync(CaseFileSearchDto spec, [FromQuery] int p = 1, CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.EnforcementManager);
        if (!UserCanViewDeletedRecords) Spec = Spec with { DeleteStatus = null };

        var paging = new PaginatedRequest(pageNumber: p, GlobalConstants.PageSize, sorting: Spec.Sort.GetDescription());
        SearchResults = await searchService.SearchAsync(Spec, paging, token: token);

        ShowResults = true;
        await PopulateSelectListsAsync(token);
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
