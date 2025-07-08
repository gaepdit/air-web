using AirWeb.AppServices.AuthenticationServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.AuthorizationPolicies.RoleRequirements.Compliance;

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
