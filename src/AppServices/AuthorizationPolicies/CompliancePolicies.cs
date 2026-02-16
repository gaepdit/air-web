using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.Domain.AppRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.AuthorizationPolicies;

public static class CompliancePolicies
{
    public static IServiceCollection AddCompliancePolicies(this IServiceCollection services)
    {
        new AuthorizationBuilder(services)
            .AddPolicy(nameof(ComplianceStaff), ComplianceStaff)
            .AddPolicy(nameof(ComplianceManager), ComplianceManager)
            .AddPolicy(nameof(EnforcementReviewer), EnforcementReviewer)
            .AddPolicy(nameof(EnforcementManager), EnforcementManager)
            .AddPolicy(nameof(ComplianceSiteMaintainer), ComplianceSiteMaintainer);

        // Requirements are added as scoped if they consume scoped services; otherwise as singletons.
        services
            .AddSingleton<IAuthorizationHandler, CaseFileViewRequirementsHandler>()
            .AddSingleton<IAuthorizationHandler, CaseFileSummaryRequirementsHandler>()
            .AddScoped<IAuthorizationHandler, FceRequirementsHandler>()
            .AddScoped<IAuthorizationHandler, ComplianceWorkRequirementsHandler>();

        return services;
    }

    // Compliance Role-based policies
    public static AuthorizationPolicy ComplianceStaff { get; } = Policies.ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsComplianceStaff()).Build();

    public static AuthorizationPolicy ComplianceManager { get; } = Policies.ActiveUserPolicyBuilder
        .RequireRole(ComplianceRole.ComplianceManager).Build();

    public static AuthorizationPolicy EnforcementReviewer { get; } = Policies.ActiveUserPolicyBuilder
        .RequireRole(ComplianceRole.EnforcementReviewer).Build();

    public static AuthorizationPolicy EnforcementManager { get; } = Policies.ActiveUserPolicyBuilder
        .RequireRole(ComplianceRole.EnforcementManager).Build();

    public static AuthorizationPolicy ComplianceSiteMaintainer { get; } = Policies.ActiveUserPolicyBuilder
        .RequireRole(ComplianceRole.ComplianceSiteMaintenance).Build();
}
