using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Sbeap.Cases.Permissions;

internal class CaseworkUpdatePermissionsHandler :
    AuthorizationHandler<CaseworkOperation, CaseworkUpdateDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseworkOperation requirement,
        CaseworkUpdateDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CaseworkOperation.Edit) =>
                // Cases can only be edited if they and the associated Customer are not deleted.
                context.User.IsSbeapStaff() && IsNotDeleted(resource),

            nameof(CaseworkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsSbeapAdmin(),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(CaseworkUpdateDto resource) =>
        resource is { IsDeleted: false, CustomerIsDeleted: false };
}
