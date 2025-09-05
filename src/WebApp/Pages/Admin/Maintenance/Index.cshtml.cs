using AirWeb.AppServices.AuthorizationPolicies;

namespace AirWeb.WebApp.Pages.Admin.Maintenance;

[Authorize(Policy = nameof(Policies.Staff))]
public class MaintenanceIndexModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
