using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.WorkEntries;
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
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser)
            .AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer)
            .AddPolicy(nameof(Policies.Staff), Policies.Staff)
            .AddPolicy(nameof(Policies.UserAdministrator), Policies.UserAdministrator)
            .AddPolicy(nameof(Policies.ComplianceStaff), Policies.ComplianceStaff)
            .AddPolicy(nameof(Policies.ComplianceManager), Policies.ComplianceManager)
            .AddPolicy(nameof(Policies.EnforcementManager), Policies.EnforcementManager)
            .AddPolicy(nameof(Policies.ComplianceSiteMaintainer), Policies.ComplianceSiteMaintainer);

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
