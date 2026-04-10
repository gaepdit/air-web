using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.Domain.Core.AppRoles;
using AirWeb.Domain.Sbeap.AppRoles;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.ActionItemTypes;

[Authorize(Policy = nameof(Policies.ViewSiteMaintenancePage))]
public class IndexModel : IndexBase
{
    public override MaintenanceOption ThisOption => MaintenanceOption.ActionItemType;
    public override AuthorizationPolicy Policy => SbeapPolicies.SbeapSiteMaintainer;
    public override AppRole AppRole => SbeapRole.SbeapSiteMaintenanceRole;

    public async Task OnGetAsync(
        [FromServices] IActionItemTypeService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization);
}
