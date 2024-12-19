using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Permissions;

internal class WorkEntryRequirement(IWorkEntryService service) :
    AuthorizationHandler<ComplianceWorkOperation, IWorkEntrySummaryDto>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        IWorkEntrySummaryDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true }) return;

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.AddComment) => context.User.CanAddComment(resource),
            nameof(ComplianceWorkOperation.Close) => context.User.CanClose(resource),
            nameof(ComplianceWorkOperation.Delete) => context.User.CanDelete(resource),
            nameof(ComplianceWorkOperation.DeleteComment) => context.User.CanDeleteComment(resource),
            nameof(ComplianceWorkOperation.Edit) => context.User.CanEdit(resource),
            nameof(ComplianceWorkOperation.Reopen) => context.User.CanReopen(resource),
            nameof(ComplianceWorkOperation.Restore) => await CanRestoreAsync(context, resource).ConfigureAwait(false),
            nameof(ComplianceWorkOperation.ViewDeleted) => context.User.CanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync(AuthorizationHandlerContext context, IWorkEntrySummaryDto resource) =>
        context.User.CanRestore(resource) &&
        (resource is not SourceTestReviewViewDto dto ||
         !await service.SourceTestReviewExistsAsync(dto.ReferenceNumber).ConfigureAwait(false));
}
