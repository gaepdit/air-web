using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.AppServices.AuthenticationServices.Roles;
using AirWeb.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.AuthorizationPolicies;

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
    public static void AddPolicies(this IServiceCollection services) => services.AddAuthorizationBuilder()
        .AddPolicy(nameof(ActiveUser), ActiveUser)
        .AddPolicy(nameof(SiteMaintainer), SiteMaintainer)
        .AddPolicy(nameof(Staff), Staff)
        .AddPolicy(nameof(UserAdministrator), UserAdministrator)
        .AddPolicy(nameof(ComplianceStaff), ComplianceStaff)
        .AddPolicy(nameof(ComplianceManager), ComplianceManager)
        .AddPolicy(nameof(EnforcementReviewer), EnforcementReviewer)
        .AddPolicy(nameof(EnforcementManager), EnforcementManager)
        .AddPolicy(nameof(ComplianceSiteMaintainer), ComplianceSiteMaintainer)
        .AddPolicy(nameof(ViewAdminPages), ViewAdminPages)
        .AddPolicy(nameof(ViewSiteMaintenancePage), ViewSiteMaintenancePage)
        .AddPolicy(nameof(ViewUsersPage), ViewUsersPage);

    // Default policy builder
    private static AuthorizationPolicyBuilder ActiveUserPolicyBuilder => new AuthorizationPolicyBuilder()
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
        .RequireRole(RoleName.AppUserAdmin).Build();

    // -- Compliance roles
    public static AuthorizationPolicy ComplianceStaff { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsComplianceStaff()).Build();

    public static AuthorizationPolicy ComplianceManager { get; } = ActiveUserPolicyBuilder
        .RequireRole(RoleName.ComplianceManager).Build();

    public static AuthorizationPolicy EnforcementReviewer { get; } = ActiveUserPolicyBuilder
        .RequireRole(RoleName.EnforcementReviewer).Build();

    public static AuthorizationPolicy EnforcementManager { get; } = ActiveUserPolicyBuilder
        .RequireRole(RoleName.EnforcementManager).Build();

    public static AuthorizationPolicy ComplianceSiteMaintainer { get; } = ActiveUserPolicyBuilder
        .RequireRole(RoleName.ComplianceSiteMaintenance).Build();

    // -- Admin page access

    public static AuthorizationPolicy ViewAdminPages { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context =>
            context.User.IsStaff() || context.User.IsSiteMaintainer() || context.User.IsUserAdmin()).Build();

    public static AuthorizationPolicy ViewSiteMaintenancePage { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff() || context.User.IsSiteMaintainer()).Build();

    public static AuthorizationPolicy ViewUsersPage { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff() || context.User.IsUserAdmin()).Build();
}
