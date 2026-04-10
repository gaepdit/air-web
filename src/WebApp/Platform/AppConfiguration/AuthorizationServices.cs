using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.Domain.Core.AppRoles;
using AirWeb.Domain.Sbeap.AppRoles;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class AuthorizationServices
{
    public static void ConfigureAuthorization(this IHostApplicationBuilder builder)
    {
        // Register application roles.
        GeneralRole.AddRoles();
        ComplianceRole.AddRoles();
        SbeapRole.AddRoles();

        builder.Services
            .AddAuthorizationPolicies()
            .AddCompliancePolicies()
            .AddSbeapPolicies()
            .AddAuthorization();
    }
}
