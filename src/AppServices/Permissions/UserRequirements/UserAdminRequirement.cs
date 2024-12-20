using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Permissions.UserRequirements;

internal class UserAdminRequirement :
    AuthorizationHandler<UserAdminRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserAdminRequirement requirement)
    {
        if (context.User.IsUserAdmin())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
