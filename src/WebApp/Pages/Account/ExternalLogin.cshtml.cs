using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Identity;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using AirWeb.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace AirWeb.WebApp.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    IStaffService staffService,
    ILogger<ExternalLoginModel> logger)
    : PageModel
{
    public ApplicationUser? DisplayFailedUser { get; private set; }
    public string? ReturnUrl { get; private set; }

    // Don't call this page directly
    public RedirectToPageResult OnGet() => RedirectToPage("./Login");

    // This Post method is called from the Login page
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;

        // Use AzureAD authentication if enabled; otherwise, sign in as local user.
        if (!AppSettings.DevSettings.UseAzureAd) return await SignInAsLocalUser();

        // Request a redirect to the external login provider.
        const string provider = OpenIdConnectDefaults.AuthenticationScheme;
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    private async Task<IActionResult> SignInAsLocalUser()
    {
        logger.LogInformation(
            "Local user signin attempted with settings {LocalUserIsAuthenticated}, {LocalUserIsAdmin}, and {LocalUserIsStaff}",
            AppSettings.DevSettings.LocalUserIsAuthenticated.ToString(),
            AppSettings.DevSettings.LocalUserIsAdmin.ToString(),
            AppSettings.DevSettings.LocalUserIsStaff.ToString());
        if (!AppSettings.DevSettings.LocalUserIsAuthenticated) return Forbid();

        StaffSearchDto search;

        if (AppSettings.DevSettings.LocalUserIsAdmin)
            search = new StaffSearchDto(SortBy.NameAsc, "Admin", null, null, null, null);
        else if (AppSettings.DevSettings.LocalUserIsStaff)
            search = new StaffSearchDto(SortBy.NameAsc, "General", null, null, null, null);
        else
            search = new StaffSearchDto(SortBy.NameAsc, "Limited", null, null, null, null);

        var staffId = (await staffService.GetListAsync(search))[0].Id;

        var user = await userManager.FindByIdAsync(staffId);
        logger.LogInformation("Local user with ID {StaffId} signed in", staffId);

        user!.MostRecentLogin = DateTimeOffset.Now;
        await userManager.UpdateAsync(user);
        await signInManager.SignInAsync(user, false);
        return LocalRedirectOrHome();
    }

    // This callback method is called by the external login provider.
    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl;

        // Handle errors returned from the external provider.
        if (remoteError is not null)
            return RedirectToLoginPageWithError($"Error from work account provider: {remoteError}");

        // Get information about the user from the external provider.
        var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo?.Principal is null)
            return RedirectToLoginPageWithError("Error loading work account information.");

        var userTenant = externalLoginInfo.Principal.GetTenantId();
        var userEmail = externalLoginInfo.Principal.GetEmail();
        if (userEmail is null || userTenant is null)
            return RedirectToLoginPageWithError("Error loading detailed work account information.");

        if (!userEmail.IsValidEmailDomain())
        {
            logger.LogWarning("User with invalid email domain attempted signin");
            return RedirectToPage("./Unavailable");
        }

        logger.LogInformation("User with object ID {ObjectId} in tenant {TenantID} successfully authenticated",
            externalLoginInfo.Principal.GetObjectId(), userTenant);

        // Determine if a user account already exists with the Object ID.
        // If not, then determine if a user account already exists with the given username.
        var user = AppSettings.DevSettings.UseInMemoryData
            ? await userManager.FindByNameAsync(userEmail)
            : await userManager.Users.SingleOrDefaultAsync(au =>
                  au.ObjectIdentifier == externalLoginInfo.Principal.GetObjectId()) ??
              await userManager.FindByNameAsync(userEmail);

        // If the user does not have a local account yet, then create one and sign in.
        if (user is null)
            return await CreateUserAndSignInAsync(externalLoginInfo);

        // If user has been marked as inactive, don't sign in.
        if (!user.Active)
        {
            logger.LogWarning("Inactive user with object ID {ObjectId} attempted signin", user.ObjectIdentifier);
            return RedirectToPage("./Unavailable");
        }

        // Try to sign in the user locally with the external provider key.
        var signInResult = await signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true, true);

        if (signInResult.IsLockedOut || signInResult.IsNotAllowed || signInResult.RequiresTwoFactor)
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("./Unavailable");
        }

        if (signInResult.Succeeded)
            return await RefreshUserInfoAndSignInAsync(user, externalLoginInfo);

        // If ExternalLoginInfo successfully returned from external provider, and the user exists, but
        // ExternalLoginSignInAsync failed, then add the external provider info to the user and sign in.
        // (`signInResult.Succeeded == false`.)
        return await AddLoginProviderAndSignInAsync(user, externalLoginInfo);
    }

    // Redirect to Login page with error message.
    private RedirectToPageResult RedirectToLoginPageWithError(string message)
    {
        logger.LogWarning("External login error: {Message}", message);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Danger, message);
        return RedirectToPage("./Login", new { ReturnUrl });
    }

    // Create a new user account and sign in.
    private async Task<IActionResult> CreateUserAndSignInAsync(ExternalLoginInfo info)
    {
        var user = new ApplicationUser
        {
            UserName = info.Principal.GetDisplayName(),
            Email = info.Principal.GetEmail(),
            GivenName = info.Principal.GetGivenName(),
            FamilyName = info.Principal.GetFamilyName(),
            ObjectIdentifier = info.Principal.GetObjectId(),
            AccountCreatedAt = DateTimeOffset.Now,
            MostRecentLogin = DateTimeOffset.Now,
        };

        // Create the user in the backing store.
        var createUserResult = await userManager.CreateAsync(user);
        if (!createUserResult.Succeeded)
        {
            logger.LogWarning("Failed to create new user with object ID {ObjectId}", user.ObjectIdentifier);
            return await FailedLoginAsync(createUserResult, user);
        }

        logger.LogInformation("Created new user with object ID {ObjectId}", user.ObjectIdentifier);

        // Add new user to application Roles if seeded in app settings or local admin user setting is enabled.
        var seedAdminUsers = configuration.GetSection("SeedAdminUsers").Get<string[]>();
        if (AppSettings.DevSettings.LocalUserIsStaff)
        {
            logger.LogInformation("Seeding staff role for new user with object ID {ObjectId}", user.ObjectIdentifier);
            await userManager.AddToRoleAsync(user, RoleName.ComplianceStaff);
            await userManager.AddToRoleAsync(user, RoleName.ComplianceSiteMaintenance);
        }

        if (AppSettings.DevSettings.LocalUserIsAdmin ||
            (seedAdminUsers != null && seedAdminUsers.Contains(user.Email, StringComparer.InvariantCultureIgnoreCase)))
        {
            logger.LogInformation("Seeding all roles for new user with object ID {ObjectId}", user.ObjectIdentifier);
            foreach (var role in AppRole.AllRoles!) await userManager.AddToRoleAsync(user, role.Key);
        }

        // Add the external provider info to the user and sign in.
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            "Your account has successfully been created. Select “Edit Profile” to update your info.");
        return await AddLoginProviderAndSignInAsync(user, info, newUser: true);
    }

    // Update local store with from external provider. 
    private async Task<IActionResult> RefreshUserInfoAndSignInAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        logger.LogInformation("Existing user with object ID {ObjectId} logged in with {LoginProvider} provider",
            user.ObjectIdentifier, info.LoginProvider);

        var previousValues = new ApplicationUser
        {
            UserName = user.UserName,
            Email = user.Email,
            GivenName = user.GivenName,
            FamilyName = user.FamilyName,
        };

        user.UserName = info.Principal.GetDisplayName();
        user.Email = info.Principal.GetEmail();
        user.GivenName = info.Principal.GetGivenName();
        user.FamilyName = info.Principal.GetFamilyName();
        user.MostRecentLogin = DateTimeOffset.Now;

        if (user.UserName != previousValues.UserName || user.Email != previousValues.Email ||
            user.GivenName != previousValues.GivenName || user.FamilyName != previousValues.FamilyName)
        {
            user.AccountUpdatedAt = DateTimeOffset.Now;
        }

        await userManager.UpdateAsync(user);
        await signInManager.RefreshSignInAsync(user);
        return LocalRedirectOrHome();
    }

    // Add external login provider to user account and sign in user.
    private async Task<IActionResult> AddLoginProviderAndSignInAsync(
        ApplicationUser user, ExternalLoginInfo info, bool newUser = false)
    {
        var addLoginResult = await userManager.AddLoginAsync(user, info);

        if (!addLoginResult.Succeeded)
        {
            logger.LogWarning("Failed to add login provider {LoginProvider} for user with object ID {ObjectId}",
                info.LoginProvider, user.ObjectIdentifier);
            return await FailedLoginAsync(addLoginResult, user);
        }

        user.ObjectIdentifier ??= info.Principal.GetObjectId();
        user.MostRecentLogin = DateTimeOffset.Now;
        await userManager.UpdateAsync(user);

        logger.LogInformation("Login provider {LoginProvider} added for user with object ID {ObjectId}",
            info.LoginProvider, user.ObjectIdentifier);

        // Include the access token in the properties.
        var props = new AuthenticationProperties();
        if (info.AuthenticationTokens is not null) props.StoreTokens(info.AuthenticationTokens);
        props.IsPersistent = true;

        await signInManager.SignInAsync(user, props, info.LoginProvider);
        // If new user, redirect to Account page
        return newUser ? RedirectToPage("./Index") : LocalRedirectOrHome();
    }

    // Add error info and return this Page.
    private async Task<PageResult> FailedLoginAsync(IdentityResult result, ApplicationUser user)
    {
        DisplayFailedUser = user;
        await signInManager.SignOutAsync();
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }

    private IActionResult LocalRedirectOrHome() =>
        ReturnUrl is null ? RedirectToPage("/Home/Index") : LocalRedirect(ReturnUrl);
}
