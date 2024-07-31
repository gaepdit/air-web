using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

public class NotificationTypeIndexModel : IndexBase
{
    public async Task OnGetAsync(
        [FromServices] INotificationTypeService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization, MaintenanceOption.NotificationType);
}
