using AirWeb.AppServices.Permissions.ComplianceStaff.UserRequirements;
using AirWeb.AppServices.Permissions.UserRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Permissions;

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
    // These policies are for use in PageModel class attributes, e.g.:
    // [Authorize(Policy = nameof(Policies.ActiveUser))]

    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(ActiveUser), ActiveUser)
            .AddPolicy(nameof(SiteMaintainer), SiteMaintainer)
            .AddPolicy(nameof(Staff), Staff)
            .AddPolicy(nameof(UserAdministrator), UserAdministrator)
            .AddPolicy(nameof(ComplianceStaff), ComplianceStaff)
            .AddPolicy(nameof(ComplianceManager), ComplianceManager)
            .AddPolicy(nameof(EnforcementManager), EnforcementManager)
            .AddPolicy(nameof(ComplianceSiteMaintainer), ComplianceSiteMaintainer);
    }

    // Default policy builder
    private static AuthorizationPolicyBuilder ActiveUserPolicyBuilder => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().AddRequirements(new ActiveUserRequirement());

    // Claims-based policies
    public static AuthorizationPolicy ActiveUser { get; } =
        ActiveUserPolicyBuilder.Build();

    // Role-based policies

    // -- General roles
    public static AuthorizationPolicy SiteMaintainer { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new SiteMaintenanceRequirement()).Build();

    public static AuthorizationPolicy Staff { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new StaffRequirement()).Build();

    public static AuthorizationPolicy UserAdministrator { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new UserAdminRequirement()).Build();

    // -- Compliance roles
    public static AuthorizationPolicy ComplianceStaff { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new ComplianceStaffRequirement()).Build();

    public static AuthorizationPolicy ComplianceManager { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new ComplianceManagerRequirement()).Build();

    public static AuthorizationPolicy EnforcementManager { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new EnforcementManagerRequirement()).Build();

    public static AuthorizationPolicy ComplianceSiteMaintainer { get; } =
        ActiveUserPolicyBuilder.AddRequirements(new ComplianceSiteMaintenanceRequirement()).Build();
}
