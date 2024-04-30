using Microsoft.AspNetCore.Authorization;
using MyApp.AppServices.Permissions.Helpers;

namespace MyApp.AppServices.Permissions.Requirements;

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
