using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.WebApp.Pages.Account;
using Microsoft.AspNetCore.Http.Extensions;

namespace AirWeb.WebApp.Pages.Shared.Components.MainMenu;

public class MainMenuViewComponent(IAuthorizationService authorization) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PageModel model) =>
        View("Default", new MenuParams
        {
            ShowStaffOptions = await authorization.Succeeded(User, Policies.Staff),
            ShowAdminOptions = await authorization.Succeeded(User, Policies.ViewAdminPages),
            ShowSiteMaintenancePage = await authorization.Succeeded(User, Policies.ViewSiteMaintenancePage),
            ShowUsersPage = await authorization.Succeeded(User, Policies.ViewUsersPage),
            ShowAccountMenu = User.Identity is { IsAuthenticated: true },
            ShowLoginLink = model is not LoginModel,
            ReturnUrl = Request.GetEncodedPathAndQuery(),
        });

    public record MenuParams
    {
        public bool ShowStaffOptions { get; init; }
        public bool ShowAdminOptions { get; init; }
        public bool ShowSiteMaintenancePage { get; init; }
        public bool ShowUsersPage { get; init; }
        public bool ShowAccountMenu { get; init; }
        public bool ShowLoginLink { get; init; }
        public string? ReturnUrl { get; init; }
    }
}
