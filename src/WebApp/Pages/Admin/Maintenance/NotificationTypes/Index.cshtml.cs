using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.Domain.Core.AppRoles;
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
