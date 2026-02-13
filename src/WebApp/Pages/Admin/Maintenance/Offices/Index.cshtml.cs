using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.Core.AppRoles;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.ViewSiteMaintenancePage))]
public class OfficeIndexModel : IndexBase
{
    public override MaintenanceOption ThisOption => MaintenanceOption.Office;
    public override AuthorizationPolicy Policy => Policies.SiteMaintainer;
    public override AppRole AppRole => GeneralRole.SiteMaintenanceRole;

    public async Task OnGetAsync(
        [FromServices] IOfficeService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization);
}
