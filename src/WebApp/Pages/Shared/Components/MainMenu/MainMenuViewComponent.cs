using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.WebApp.Pages.Account;

namespace AirWeb.WebApp.Pages.Shared.Components.MainMenu;

public class MainMenuViewComponent(IAuthorizationService authorization) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PageModel model) =>
        View("Default", new MenuParams
        {
            ShowSearchMenu = await authorization.Succeeded(User, Policies.Staff),
            ShowMoreMenu = await authorization.Succeeded(User, Policies.ViewAdminPages),
            ShowSiteMaintenanceMenu = await authorization.Succeeded(User, Policies.ViewSiteMaintenancePage),
            ShowUsersMenu = await authorization.Succeeded(User, Policies.ViewUsersPage),
            ShowAccountMenu = User.Identity is { IsAuthenticated: true },
            ShowLoginLink = model is not LoginModel,
        });

    public record MenuParams
    {
        public bool ShowSearchMenu { get; init; }
        public bool ShowMoreMenu { get; init; }
        public bool ShowSiteMaintenanceMenu { get; init; }
        public bool ShowUsersMenu { get; init; }
        public bool ShowAccountMenu { get; init; }
        public bool ShowLoginLink { get; init; }
    }
}
