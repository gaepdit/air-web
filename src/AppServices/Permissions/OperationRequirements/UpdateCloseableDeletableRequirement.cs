using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Permissions.OperationRequirements;

public class UpdateCloseableDeletableRequirement :
    AuthorizationHandler<UpdateCloseableDeletableRequirement, IIsClosedAndIsDeleted>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UpdateCloseableDeletableRequirement requirement,
        IIsClosedAndIsDeleted resource)
    {
        if (context.User.CanEdit(resource))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
