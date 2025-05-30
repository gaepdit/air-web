using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Permissions.ComplianceStaff.UserRequirements;

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
