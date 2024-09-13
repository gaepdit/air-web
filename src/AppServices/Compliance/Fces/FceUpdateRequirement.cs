using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces;

public class FceUpdateRequirement :
    AuthorizationHandler<FceUpdateRequirement, FceUpdateDto>,
    IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FceUpdateRequirement requirement,
        FceUpdateDto resource)
    {
        if (context.User.IsComplianceStaff() && !resource.IsDeleted)
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
