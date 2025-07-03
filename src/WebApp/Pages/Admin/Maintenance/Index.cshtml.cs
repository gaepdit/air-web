using AirWeb.AppServices.AuthorizationPolicies;

namespace AirWeb.WebApp.Pages.Admin.Maintenance;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class MaintenanceIndexModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
