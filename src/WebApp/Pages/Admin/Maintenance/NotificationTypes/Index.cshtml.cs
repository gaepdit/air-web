using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.NotificationTypes;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class IndexModel : PageModel
{
    public IReadOnlyList<NotificationTypeViewDto> Items { get; private set; } = default!;
    public static MaintenanceOption ThisOption => MaintenanceOption.NotificationType;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] INotificationTypeService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        IsSiteMaintainer = await authorization.Succeeded(User, Policies.SiteMaintainer);
    }
}
