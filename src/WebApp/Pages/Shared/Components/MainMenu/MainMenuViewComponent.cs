﻿using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Pages.Account;

namespace AirWeb.WebApp.Pages.Shared.Components.MainMenu;

public class MainMenuViewComponent(IAuthorizationService authorization) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PageModel model) =>
        View("Default", new MenuParams
        {
            IsLoginPage = model is LoginModel,
            IsActiveUser = await authorization.Succeeded(User, Policies.ActiveUser),
            IsStaffUser = await authorization.Succeeded(User, Policies.ComplianceStaff),
        });

    public record MenuParams
    {
        public bool IsLoginPage { get; init; }
        public bool IsActiveUser { get; init; }
        public bool IsStaffUser { get; init; }
    }
}
