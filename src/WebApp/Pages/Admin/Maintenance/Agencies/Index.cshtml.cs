using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.Domain.Core.AppRoles;
using AirWeb.Domain.Sbeap.AppRoles;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Agencies;

[Authorize(Policy = nameof(Policies.ViewSiteMaintenancePage))]
public class IndexModel : IndexBase
{
    public override MaintenanceOption ThisOption => MaintenanceOption.Agency;
    public override AuthorizationPolicy Policy => SbeapPolicies.SbeapSiteMaintainer;
    public override AppRole AppRole => SbeapRole.SbeapSiteMaintenanceRole;

    public async Task OnGetAsync(
        [FromServices] IAgencyService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization);
}
