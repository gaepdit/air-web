using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.Data;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.SBEAP.Customers;

[Authorize(Policy = nameof(SbeapPolicies.SbeapStaff))]
public class IndexModel(ICustomerService service, IAuthorizationService authorization)
    : PageModel
{
    // Properties
    public CustomerSearchDto Spec { get; set; } = null!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<CustomerSearchResultDto> SearchResults { get; private set; } = null!;
    public string SortByName => Spec.Sort.ToString();
    public bool ShowDeletionSearchOptions { get; private set; }
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    // Select lists
    public static SelectList CountiesSelectList => new(CommonData.Counties);
    public SelectList SicSelectList { get; private set; } = null!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        ShowDeletionSearchOptions = await UserCanManageDeletionsAsync();
        PopulateSelectLists();
        return Page();
    }

    public async Task<IActionResult> OnGetSearchAsync(CustomerSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();

        ShowDeletionSearchOptions = await UserCanManageDeletionsAsync();
        if (!ShowDeletionSearchOptions) Spec = Spec with { DeletedStatus = null };

        var paging = new PaginatedRequest(p, SearchDefaults.PageSize, Spec.Sort.GetDescription());
        SearchResults = await service.SearchAsync(Spec, paging);

        ShowResults = true;
        PopulateSelectLists();
        return Page();
    }

    private void PopulateSelectLists() =>
        SicSelectList = SicCodes.ActiveListItems.ToSelectList();

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(SbeapPolicies.SbeapAdmin))).Succeeded;
}
