using AirWeb.AppServices.Compliance.Permissions;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces;

internal class FceViewRequirement(IFceService service) :
    AuthorizationHandler<ComplianceWorkOperation, FceViewDto>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        FceViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true }) return;

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.AddComment) => ComplianceWorkOperation.CanAddComment(context.User, resource),
            nameof(ComplianceWorkOperation.Delete) => ComplianceWorkOperation.CanDelete(context.User, resource),
            nameof(ComplianceWorkOperation.DeleteComment) => ComplianceWorkOperation
                .CanDeleteComment(context.User, resource),
            nameof(ComplianceWorkOperation.Edit) => ComplianceWorkOperation.CanEdit(context.User, resource),
            nameof(ComplianceWorkOperation.Restore) => await CanRestoreAsync(context, resource).ConfigureAwait(false),
            nameof(ComplianceWorkOperation.ViewDeleted) => ComplianceWorkOperation.CanManageDeletions(context.User),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync(AuthorizationHandlerContext context, FceViewDto resource) =>
        ComplianceWorkOperation.CanRestore(context.User, resource) &&
        !await service.ExistsAsync((FacilityId)resource.FacilityId, resource.Year, resource.Id).ConfigureAwait(false);
}
