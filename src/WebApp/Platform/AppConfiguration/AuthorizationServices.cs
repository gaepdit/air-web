using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Domain.Compliance.AppRoles;
using AirWeb.Domain.Core.AppRoles;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class AuthorizationServices
{
    public static void ConfigureAuthorization(this IHostApplicationBuilder builder)
    {
        // Register application roles.
        GeneralRole.AddRoles();
        ComplianceRole.AddRoles();

        builder.Services
            .AddAuthorizationPolicies()
            .AddCompliancePolicies()
            .AddAuthorization();
    }
}
