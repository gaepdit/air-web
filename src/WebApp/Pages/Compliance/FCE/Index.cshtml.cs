using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Compliance.FCE;

[Authorize(Policy = nameof(Policies.Staff))]
public class FceIndexModel(
    IFceSearchService searchService,
    IStaffService staff,
    IOfficeService offices,
    IAuthorizationService authorization)
    : PageModel
{
    public FceSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public bool UserCanViewDeletedRecords { get; private set; }
    public IPaginatedResult<FceSearchResultDto> SearchResults { get; private set; } = null!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    // Select lists
    public SelectList StaffSelectList { get; private set; } = null!;
    public SelectList OfficesSelectList { get; set; } = null!;

    private static int _finalYear = DateTime.Today.Month > 9 ? DateTime.Now.Year + 1 : DateTime.Now.Year;
    private static int _years = _finalYear - Fce.EarliestFceYear + 1;
    public static SelectList YearSelectList => new(Enumerable.Range(Fce.EarliestFceYear, _years).Reverse());

    public async Task OnGetAsync(CancellationToken token = default)
    {
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.ComplianceManager);
        await PopulateSelectListsAsync(token);
    }

    public async Task OnGetSearchAsync(FceSearchDto spec, [FromQuery] int p = 1, CancellationToken token = default)
    {
        Spec = spec.TrimAll();
        UserCanViewDeletedRecords = await authorization.Succeeded(User, Policies.ComplianceManager);
        if (!UserCanViewDeletedRecords) Spec = Spec with { DeleteStatus = null };

        var paging = new PaginatedRequest(pageNumber: p, GlobalConstants.PageSize, sorting: Spec.Sort.GetDescription());
        SearchResults = await searchService.SearchAsync(Spec, paging, token: token);

        ShowResults = true;
        await PopulateSelectListsAsync(token);
    }

    private async Task PopulateSelectListsAsync(CancellationToken token)
    {
        StaffSelectList = (await staff.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        OfficesSelectList = (await offices.GetAsListItemsAsync(includeInactive: true, token: token)).ToSelectList();
    }
}
