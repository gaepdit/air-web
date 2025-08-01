﻿using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.WebApp.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    public Task<SignOutResult> OnGetAsync() => SignOut();
    public Task<SignOutResult> OnPostAsync() => SignOut();

    private async Task<SignOutResult> SignOut()
    {
        var authenticationProperties = new AuthenticationProperties { RedirectUri = "../" };
        var userAuthenticationScheme = User.GetAuthenticationMethod();

        if (userAuthenticationScheme is null or LoginProviders.TestUserScheme)
        {
            await signInManager.SignOutAsync();
            return SignOut(authenticationProperties);
        }

        List<string> authenticationSchemes = [CookieAuthenticationDefaults.AuthenticationScheme];

        authenticationSchemes.AddRange([IdentityConstants.ApplicationScheme, userAuthenticationScheme]);

        return SignOut(authenticationProperties, authenticationSchemes.ToArray());
    }
}
