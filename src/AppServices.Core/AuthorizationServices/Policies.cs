using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.Core.AppRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Core.AuthorizationServices;

#pragma warning disable S125 // Sections of code should not be commented out
//
// Two ways to use these policies:
//
// A. As an attribute on a PageModel class (must be registered first in `AddAuthorizationPolicies`):
//
//    [Authorize(Policy = nameof(Policies.ActiveUser))]
//    public class AddModel : PageModel
//
// B. From a DI authorization service: 
//
//    public async Task<IActionResult> OnGetAsync([FromServices] IAuthorizationService authorization)
//    {
//        var isStaff = (await authorization.AuthorizeAsync(User, Policies.StaffUser)).Succeeded;
//
//        // or, with `using AuthorizationServiceExtensions;`:
//        var isStaff = await authorization.Succeeded(User, Policies.StaffUser);
//    }
//
#pragma warning restore S125

public static class Policies
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(ActiveUser), ActiveUser)
            .AddPolicy(nameof(SiteMaintainer), SiteMaintainer)
            .AddPolicy(nameof(Staff), Staff)
            .AddPolicy(nameof(UserAdministrator), UserAdministrator)
            .AddPolicy(nameof(ViewAdminPages), ViewAdminPages)
            .AddPolicy(nameof(ViewSiteMaintenancePage), ViewSiteMaintenancePage)
            .AddPolicy(nameof(ViewUsersPage), ViewUsersPage);
        return services;
    }

    // Default policy builder
    public static AuthorizationPolicyBuilder ActiveUserPolicyBuilder => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim(AppClaimTypes.ActiveUser, true.ToString());

    // Claims-based policies
    public static AuthorizationPolicy ActiveUser { get; } = ActiveUserPolicyBuilder.Build();

    // Role-based policies

    // -- General roles
    public static AuthorizationPolicy SiteMaintainer { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsSiteMaintainer()).Build();

    public static AuthorizationPolicy Staff { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff()).Build();

    public static AuthorizationPolicy UserAdministrator { get; } = ActiveUserPolicyBuilder
        .RequireRole(GeneralRole.AppUserAdmin).Build();

    // -- Admin page access

    public static AuthorizationPolicy ViewAdminPages { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context =>
            context.User.IsStaff() || context.User.IsSiteMaintainer() || context.User.IsUserAdmin()).Build();

    public static AuthorizationPolicy ViewSiteMaintenancePage { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff() || context.User.IsSiteMaintainer()).Build();

    public static AuthorizationPolicy ViewUsersPage { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff() || context.User.IsUserAdmin()).Build();
}
