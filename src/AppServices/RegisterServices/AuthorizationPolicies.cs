using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.AppClaims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.AppServices.RegisterServices;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
public static class AuthorizationPolicies
{
    public static void AddAuthorizationHandlers(this IServiceCollection services)
    {
        services.AddAuthorizationPolicies();

        // Resource/operation-based permission handlers, e.g.:
        // var canAssign = await authorization.Succeeded(User, entryView, WorkEntryOperation.EditWorkEntry);

        // ViewRequirements are added scoped if they consume scoped services.
        services.AddScoped<IAuthorizationHandler, EnforcementRequirementsHandler>();
        services.AddScoped<IAuthorizationHandler, FceRequirementsHandler>();
        services.AddScoped<IAuthorizationHandler, WorkEntryRequirementsHandler>();

        // Add claims transformations
        services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();
    }
}
