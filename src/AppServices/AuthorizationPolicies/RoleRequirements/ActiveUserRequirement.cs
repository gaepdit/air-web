using AirWeb.AppServices.IdentityServices.Roles;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.AuthorizationPolicies.RoleRequirements;

internal class ActiveUserRequirement :
    AuthorizationHandler<ActiveUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (context.User.IsActive())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
