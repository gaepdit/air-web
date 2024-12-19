using AirWeb.AppServices.Compliance.Fces;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Permissions;

internal class FceRequirementsHandler(IFceService service) :
    AuthorizationHandler<ComplianceOperation, IFceBasicViewDto>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceOperation requirement,
        IFceBasicViewDto resource)
    {
        var user = context.User;
        if (user.Identity is not { IsAuthenticated: true }) return;

        var success = requirement.Name switch
        {
            nameof(ComplianceOperation.AddComment) => user.CanAddComment(resource),
            nameof(ComplianceOperation.Delete) => user.CanDelete(resource),
            nameof(ComplianceOperation.DeleteComment) => user.CanDeleteComment(resource),
            nameof(ComplianceOperation.Edit) => user.CanEdit(resource),
            nameof(ComplianceOperation.Restore) => await CanRestoreAsync(user, resource).ConfigureAwait(false),
            nameof(ComplianceOperation.ViewDeleted) => user.CanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync(ClaimsPrincipal user, IFceBasicViewDto item) =>
        user.CanRestore(item) &&
        !await service.ExistsAsync((FacilityId)item.FacilityId, item.Year, item.Id).ConfigureAwait(false);
}
