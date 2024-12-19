using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Permissions.OperationRequirements;

public class UpdateDeletableRequirement :
    AuthorizationHandler<UpdateDeletableRequirement, IIsDeleted>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UpdateDeletableRequirement requirement,
        IIsDeleted resource)
    {
        if (context.User.CanEdit(resource))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
