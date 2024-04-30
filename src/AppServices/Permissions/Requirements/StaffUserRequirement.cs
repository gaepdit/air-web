using Microsoft.AspNetCore.Authorization;
using MyApp.AppServices.Permissions.Helpers;

namespace MyApp.AppServices.Permissions.Requirements;

internal class StaffUserRequirement :
    AuthorizationHandler<StaffUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        StaffUserRequirement requirement)
    {
        if (context.User.IsStaff())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
