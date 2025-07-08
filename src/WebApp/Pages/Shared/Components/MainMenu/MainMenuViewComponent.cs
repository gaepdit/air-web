using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.WebApp.Pages.Account;

namespace AirWeb.WebApp.Pages.Shared.Components.MainMenu;

public class MainMenuViewComponent(IAuthorizationService authorization) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PageModel model) =>
        View("Default", new MenuParams
        {
            IsLoginPage = model is LoginModel,
            IsActiveUser = await authorization.Succeeded(User, Policies.ActiveUser),
        });

    public record MenuParams
    {
        public bool IsLoginPage { get; init; }
        public bool IsActiveUser { get; init; }
    }
}
