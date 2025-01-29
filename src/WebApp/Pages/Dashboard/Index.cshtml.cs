using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Dashboard;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DashboardIndexModel(
    IWorkEntrySearchService entrySearchService,
    IAuthorizationService authorization,
    IStaffService staffService)
    : PageModel
{
    public bool IsStaff { get; private set; }
    public bool IsComplianceStaff { get; private set; }
    public List<DashboardCard> DashboardCards { get; private set; } = [];

    public async Task<PageResult> OnGetAsync(CancellationToken token)
    {
        IsStaff = await authorization.Succeeded(User, Policies.Staff);
        if (!IsStaff) return Page();

        var currentUser = await staffService.GetCurrentUserAsync();

        // Compliance dashboard
        IsComplianceStaff = await authorization.Succeeded(User, Policies.ComplianceStaff);
        var spec = new WorkEntrySearchDto
            { Closed = ClosedOpenAny.Open, ResponsibleStaff = currentUser.Id };
        var paging = new PaginatedRequest(1, 15, SortBy.EventDateDesc.GetDescription());
        DashboardCards.Add(new DashboardCard("Open Compliance Work")
            { WorkEntries = (await entrySearchService.SearchAsync(spec, paging, token: token)).Items.ToList() });

        return Page();
    }

    public record DashboardCard(string Title)
    {
        public required IReadOnlyCollection<WorkEntrySearchResultDto> WorkEntries { get; init; }
        public string CardId => Title.ToLowerInvariant().Replace(oldChar: ' ', newChar: '-');
    }
}
