using AirWeb.WebApp.Platform.OrgNotifications;

namespace AirWeb.WebApp.Pages.Shared.Components.Notifications;

public class NotificationsViewComponent(IOrgNotifications orgNotifications) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() =>
        View("Default", await orgNotifications.GetOrgNotificationsAsync());
}