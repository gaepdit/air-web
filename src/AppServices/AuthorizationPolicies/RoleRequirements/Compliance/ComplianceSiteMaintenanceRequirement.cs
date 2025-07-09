using AirWeb.AppServices.AuthenticationServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.AuthorizationPolicies.RoleRequirements.Compliance;

internal class ComplianceSiteMaintenanceRequirement :
    AuthorizationHandler<ComplianceSiteMaintenanceRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceSiteMaintenanceRequirement requirement)
    {
        if (context.User.IsComplianceSiteMaintainer())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
