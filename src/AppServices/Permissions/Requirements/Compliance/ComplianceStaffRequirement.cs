using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Permissions.Requirements.Compliance;

internal class ComplianceStaffRequirement :
    AuthorizationHandler<ComplianceStaffRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceStaffRequirement requirement)
    {
        if (context.User.IsComplianceStaff())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
