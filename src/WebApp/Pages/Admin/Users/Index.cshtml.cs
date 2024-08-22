using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class UsersIndexModel(IOfficeService officeService, IStaffService staffService) : PageModel
{
    public StaffSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<StaffSearchResultDto> SearchResults { get; private set; } = default!;
    public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults);

    public SelectList RolesSelectList { get; private set; } = default!;
    public SelectList OfficesSelectList { get; private set; } = default!;

    public Task OnGetAsync() => PopulateSelectListsAsync();

    public async Task<IActionResult> OnGetSearchAsync(StaffSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        await PopulateSelectListsAsync();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await staffService.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        OfficesSelectList = (await officeService.GetAsListItemsAsync(includeInactive: true)).ToSelectList();
        RolesSelectList = AppRole.AllRoles!
            .Select(pair => new ListItem<string>(pair.Key, pair.Value.DisplayName))
            .ToSelectList();
    }
}
