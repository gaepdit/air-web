using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Core.AppRoles;
using AirWeb.Domain.AppRoles;

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
