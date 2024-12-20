using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Permissions.UserRequirements.Compliance;

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
