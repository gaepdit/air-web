using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Permissions;

internal class WorkEntryRequirementsHandler(IWorkEntryService service) :
    AuthorizationHandler<ComplianceOperation, IWorkEntrySummaryDto>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceOperation requirement,
        IWorkEntrySummaryDto resource)
    {
        var user = context.User;
        if (user.Identity is not { IsAuthenticated: true }) return;

        var success = requirement.Name switch
        {
            nameof(ComplianceOperation.AddComment) => user.CanAddComment(resource),
            nameof(ComplianceOperation.Close) => user.CanClose(resource),
            nameof(ComplianceOperation.Delete) => user.CanDelete(resource),
            nameof(ComplianceOperation.DeleteComment) => user.CanDeleteComment(resource),
            nameof(ComplianceOperation.Edit) => user.CanEdit(resource),
            nameof(ComplianceOperation.Reopen) => user.CanReopen(resource),
            nameof(ComplianceOperation.Restore) => await CanRestoreAsync(user, resource).ConfigureAwait(false),
            nameof(ComplianceOperation.ViewDeleted) => user.CanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync(ClaimsPrincipal user, IWorkEntrySummaryDto resource) =>
        user.CanRestore(resource) &&
        (resource is not SourceTestReviewViewDto dto ||
         !await service.SourceTestReviewExistsAsync(dto.ReferenceNumber).ConfigureAwait(false));
}
