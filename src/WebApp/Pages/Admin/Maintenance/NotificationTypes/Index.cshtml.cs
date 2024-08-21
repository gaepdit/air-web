using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.Permissions;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

public class NotificationTypeIndexModel : IndexBase
{
    public override MaintenanceOption ThisOption => MaintenanceOption.NotificationType;
    public override AuthorizationPolicy Policy => Policies.ComplianceSiteMaintainer;
    public override AppRole AppRole => AppRole.ComplianceSiteMaintenanceRole;

    public async Task OnGetAsync(
        [FromServices] INotificationTypeService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization);
}
