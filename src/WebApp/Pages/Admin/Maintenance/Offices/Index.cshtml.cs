using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class OfficeIndexModel : IndexBase
{
    public override MaintenanceOption ThisOption => MaintenanceOption.Office;
    public override AuthorizationPolicy Policy => Policies.SiteMaintainer;
    public override AppRole AppRole => AppRole.SiteMaintenanceRole;

    public async Task OnGetAsync(
        [FromServices] IOfficeService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization);
}
