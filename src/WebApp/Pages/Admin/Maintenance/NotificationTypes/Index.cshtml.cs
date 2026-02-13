using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.Core.AppRoles;
using AirWeb.Domain.AppRoles;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

[Authorize(Policy = nameof(Policies.ViewSiteMaintenancePage))]
public class NotificationTypeIndexModel : IndexBase
{
    public override MaintenanceOption ThisOption => MaintenanceOption.NotificationType;
    public override AuthorizationPolicy Policy => CompliancePolicies.ComplianceSiteMaintainer;
    public override AppRole AppRole => ComplianceRole.ComplianceSiteMaintenanceRole;

    public async Task OnGetAsync(
        [FromServices] INotificationTypeService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization);
}
