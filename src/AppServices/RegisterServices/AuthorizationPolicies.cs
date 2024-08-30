using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.WorkEntries;
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

        // FceViewRequirement is added scoped because it consumes the scoped IFceService.
        services.AddScoped<IAuthorizationHandler, FceViewRequirement>();

        // services.AddSingleton<IAuthorizationHandler, WorkEntryUpdateRequirement>();
        services.AddSingleton<IAuthorizationHandler, WorkEntryViewRequirement>();

        // Add claims transformations
        services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();
    }
}
