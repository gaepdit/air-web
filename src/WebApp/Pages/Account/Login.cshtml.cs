﻿using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.WebApp.Pages.Account;

[AllowAnonymous]
public class LoginModel(
    SignInManager<ApplicationUser> signInManager,
    IAuthenticationManager authenticationManager,
    IConfiguration configuration
) : PageModel
{
    public string? ReturnUrl { get; private set; }
    public IEnumerable<string> LoginProviderNames { get; private set; } = null!;
    public bool DisplayFailedLogin { get; private set; }

    public IActionResult OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        LoginProviderNames = configuration.LoginProviderNames();
        if (User.Identity is not { IsAuthenticated: true })
            return Page();

        return User.IsActive() ? LocalRedirectOrHome() : RedirectToPage("Logout");
    }

    public async Task<IActionResult> OnPostTestUserAsync(string? returnUrl = null)
    {
        if (!AppSettings.DevSettings.EnableTestUser) return BadRequest();
        if (!AppSettings.DevSettings.TestUserIsAuthenticated) return Forbid();

        ReturnUrl = returnUrl;
        await authenticationManager.LogInAsTestUserAsync(AppSettings.DevSettings.TestUserRoles);
        return LocalRedirectOrHome();
    }

    public IActionResult OnPostAsync(string scheme, string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true }) return RedirectToPage("Logout");
        if (!configuration.ValidateLoginProvider(scheme))
            throw new ArgumentException("Invalid scheme", nameof(scheme));

        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("Login", pageHandler: "Callback", values: new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);
        return Challenge(properties, scheme);
    }

    // The callback method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl;
        if (remoteError is not null)
            return LoginPageWithError($"Error from account provider: {remoteError}");
        var result = await authenticationManager.LogInUsingExternalProviderAsync();
        return result.Succeeded ? LocalRedirectOrHome() : await FailedLoginAsync(result);
    }

    private RedirectToPageResult LoginPageWithError(string message)
    {
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Danger, message);
        return RedirectToPage("Login", new { ReturnUrl });
    }

    private async Task<PageResult> FailedLoginAsync(IdentityResult result)
    {
        await signInManager.SignOutAsync();
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        DisplayFailedLogin = true;
        return Page();
    }

    private IActionResult LocalRedirectOrHome() =>
        ReturnUrl is null ? RedirectToPage("/Index") : LocalRedirect(ReturnUrl);
}
