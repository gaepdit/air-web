using AirWeb.AppServices.IdentityServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.AuthorizationPolicies.RoleRequirements.Compliance;

internal class ComplianceManagerRequirement :
    AuthorizationHandler<ComplianceManagerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceManagerRequirement requirement)
    {
        if (context.User.IsComplianceManager())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
