using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Permissions;

internal class FceViewRequirementHandler :
    AuthorizationHandler<ComplianceWorkOperation, FceViewDto>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        FceViewDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.Edit) =>
                // FCEs can only be edited if they are not deleted.
                context.User.IsStaff() && IsNotDeleted(resource),

            nameof(ComplianceWorkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsManager(),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(FceViewDto resource) => !resource.IsDeleted;
}
