using AirWeb.AppServices.AuthenticationServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.AuthorizationPolicies.RoleRequirements.Compliance;

internal class EnforcementManagerRequirement :
    AuthorizationHandler<EnforcementManagerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EnforcementManagerRequirement requirement)
    {
        if (context.User.IsEnforcementManager())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
