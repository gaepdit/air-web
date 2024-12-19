using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces;

internal class FceRequirement(IFceService service) :
    AuthorizationHandler<ComplianceWorkOperation, IFceBasicViewDto>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        IFceBasicViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true }) return;

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.AddComment) => context.User.CanAddComment(resource),
            nameof(ComplianceWorkOperation.Delete) => context.User.CanDelete(resource),
            nameof(ComplianceWorkOperation.DeleteComment) => context.User.CanDeleteComment(resource),
            nameof(ComplianceWorkOperation.Edit) => context.User.CanEdit(resource),
            nameof(ComplianceWorkOperation.Restore) => await CanRestoreAsync(context, resource).ConfigureAwait(false),
            nameof(ComplianceWorkOperation.ViewDeleted) => context.User.CanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync(AuthorizationHandlerContext context, IFceBasicViewDto item) =>
        context.User.CanRestore(item) &&
        !await service.ExistsAsync((FacilityId)item.FacilityId, item.Year, item.Id).ConfigureAwait(false);
}
