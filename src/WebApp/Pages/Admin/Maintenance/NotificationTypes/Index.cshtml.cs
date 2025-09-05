using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

[Authorize(Policy = nameof(Policies.Staff))]
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
