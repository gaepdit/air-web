using AirWeb.AppServices.Core.AuthorizationServices;

namespace AirWeb.WebApp.Pages.Admin.Reports;

[Authorize(Policy = nameof(Policies.Staff))]
public class ReportsIndexModel() : PageModel
{
    public string CurrentView { get; private set; } = "Menu";
    // public IReadOnlyCollection<FacilityFceStatus> FacilityFces { get; private set; } = null!;

    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task OnGetMostRecentFCEs()
    {
        CurrentView = nameof(OnGetMostRecentFCEs);
    }
}
