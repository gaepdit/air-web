using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Cases;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.SBEAP.Cases;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class IndexModel(ICaseworkService service, IAgencyService agencyService, IAuthorizationService authorization)
    : PageModel
{
    // Properties
    public CaseworkSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<CaseworkSearchResultDto> SearchResults { get; private set; } = null!;
    public string SortByName => Spec.Sort.ToString();
    public bool ShowDeletionSearchOptions { get; private set; }
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

    // Select lists
    public SelectList AgencySelectList { get; private set; } = null!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        ShowDeletionSearchOptions = await UserCanManageDeletionsAsync();
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnGetSearchAsync(CaseworkSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();

        ShowDeletionSearchOptions = await UserCanManageDeletionsAsync();
        if (!ShowDeletionSearchOptions) Spec = Spec with { DeletedStatus = null, CustomerDeletedStatus = null };

        var paging = new PaginatedRequest(p, SearchDefaults.PageSize, Spec.Sort.GetDescription());
        SearchResults = await service.SearchAsync(Spec, paging);

        ShowResults = true;
        await PopulateSelectListsAsync();
        return Page();
    }

    private async Task PopulateSelectListsAsync() =>
        AgencySelectList = (await agencyService.GetAsListItemsAsync(true)).ToSelectList();

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(SbeapPolicies.SbeapAdmin))).Succeeded;
}
