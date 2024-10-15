using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Permissions;

internal class WorkEntryViewRequirement(IWorkEntryService service) :
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
            nameof(ComplianceWorkOperation.AddComment) => ComplianceWorkOperation.CanAddComment(context.User, resource),
            nameof(ComplianceWorkOperation.Close) => ComplianceWorkOperation.CanClose(context.User, resource),
            nameof(ComplianceWorkOperation.Delete) => ComplianceWorkOperation.CanDelete(context.User, resource),
            nameof(ComplianceWorkOperation.DeleteComment) => ComplianceWorkOperation.CanDeleteComment(context.User,
                resource),
            nameof(ComplianceWorkOperation.Edit) => ComplianceWorkOperation.CanEdit(context.User, resource),
            nameof(ComplianceWorkOperation.Reopen) => ComplianceWorkOperation.CanReopen(context.User, resource),
            nameof(ComplianceWorkOperation.Restore) => await CanRestoreAsync(context, resource).ConfigureAwait(false),
            nameof(ComplianceWorkOperation.ViewDeleted) => ComplianceWorkOperation.CanManageDeletions(context.User),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync(AuthorizationHandlerContext context, IWorkEntrySummaryDto resource) =>
        ComplianceWorkOperation.CanRestore(context.User, resource) &&
        (resource is not SourceTestReviewViewDto dto ||
         !await service.SourceTestReviewExistsAsync(dto.ReferenceNumber).ConfigureAwait(false));
}
