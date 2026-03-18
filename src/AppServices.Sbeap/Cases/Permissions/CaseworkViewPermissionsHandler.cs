using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Sbeap.Cases.Permissions;

internal class CaseworkViewPermissionsHandler :
    AuthorizationHandler<CaseworkOperation, CaseworkViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseworkOperation requirement,
        CaseworkViewDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CaseworkOperation.Edit) =>
                // Cases can only be edited if they and the associated Customer are not deleted.
                context.User.IsSbeapStaff() && IsNotDeleted(resource),

            nameof(CaseworkOperation.EditActionItems) =>
                // Action Items can only be edited if the Case is still open.
                context.User.IsSbeapStaff() && IsOpen(resource) && IsNotDeleted(resource),

            nameof(CaseworkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsSbeapAdmin() && CustomerIsNotDeleted(resource),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(CaseworkViewDto resource) => !resource.IsDeleted && CustomerIsNotDeleted(resource);
    private static bool CustomerIsNotDeleted(CaseworkViewDto resource) => !resource.Customer.IsDeleted;
    private static bool IsOpen(CaseworkViewDto resource) => resource.CaseClosedDate is null;
}
