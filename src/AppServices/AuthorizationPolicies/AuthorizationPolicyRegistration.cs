using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Enforcement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.AppServices.AuthorizationPolicies;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
public static class AuthorizationPolicyRegistration
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddPolicies();

        // Resource/operation-based permission handlers, e.g.:
        // var canAssign = await authorization.Succeeded(User, entryView, WorkEntryOperation.EditWorkEntry);

        // ViewRequirements are added as scoped if they consume scoped services; otherwise as singletons.
        services
            .AddSingleton<IAuthorizationHandler, CaseFileViewRequirementsHandler>()
            .AddSingleton<IAuthorizationHandler, CaseFileSummaryRequirementsHandler>()
            .AddScoped<IAuthorizationHandler, FceRequirementsHandler>()
            .AddScoped<IAuthorizationHandler, WorkEntryRequirementsHandler>();

        return services;
    }
}
