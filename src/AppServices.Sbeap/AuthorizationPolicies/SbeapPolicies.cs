using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Sbeap.Cases.Permissions;
using AirWeb.AppServices.Sbeap.Customers.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Sbeap.AuthorizationPolicies;

#pragma warning disable S125 // Sections of code should not be commented out
//
// Two ways to use these policies:
//
// A. As an attribute on a PageModel class:
//
//    [Authorize(Policy = PolicyName.SiteMaintainer)]
//    public class AddModel : PageModel
//
// B. From a DI authorization service: 
//
//    public async Task<IActionResult> OnGetAsync([FromServices] IAuthorizationService authorizationService)
//    {
//        var isStaff = (await authorizationService.AuthorizeAsync(User, Policies.StaffUserPolicy())).Succeeded;
//    }
//
#pragma warning restore S125

public static class SbeapPolicies
{
    public static IServiceCollection AddSbeapPolicies(this IServiceCollection services)
    {
        new AuthorizationBuilder(services)
            .AddPolicy(nameof(SbeapStaffOrMaintainer), SbeapStaffOrMaintainer)
            .AddPolicy(nameof(SbeapAdmin), SbeapAdmin)
            .AddPolicy(nameof(SbeapSiteMaintainer), SbeapSiteMaintainer)
            .AddPolicy(nameof(SbeapStaff), SbeapStaff);

        // ViewRequirements are added as scoped if they consume scoped services; otherwise as singletons.
        services
            .AddSingleton<IAuthorizationHandler, ActionItemUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CaseworkUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CaseworkViewPermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, ContactUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CustomerUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CustomerViewPermissionsHandler>();

        return services;
    }

    // Claims-based policies
    public static AuthorizationPolicy ActiveUser { get; } = Policies.ActiveUserPolicyBuilder.Build();

    // Role-based policies
    public static AuthorizationPolicy SbeapStaffOrMaintainer { get; } = Policies.ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsSbeapStaffOrMaintainer()).Build();

    public static AuthorizationPolicy SbeapAdmin { get; } = Policies.ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsSbeapAdmin()).Build();

    public static AuthorizationPolicy SbeapSiteMaintainer { get; } = Policies.ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsSbeapSiteMaintainer()).Build();

    public static AuthorizationPolicy SbeapStaff { get; } = Policies.ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff()).Build();
}
