using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Permissions;

internal class WorkEntryViewRequirement :
    AuthorizationHandler<ComplianceWorkOperation, IWorkEntryViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        IWorkEntryViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.AddComment) => ComplianceWorkOperation.CanAddComment(context.User, resource),
            nameof(ComplianceWorkOperation.Close) => ComplianceWorkOperation.CanClose(context.User, resource),
            nameof(ComplianceWorkOperation.Delete) => ComplianceWorkOperation.CanDelete(context.User, resource),
            nameof(ComplianceWorkOperation.DeleteComment) => ComplianceWorkOperation.CanDeleteComment(context.User,
                resource),
            nameof(ComplianceWorkOperation.Edit) => ComplianceWorkOperation.CanEdit(context.User, resource),
            nameof(ComplianceWorkOperation.Reopen) => ComplianceWorkOperation.CanReopen(context.User, resource),
            nameof(ComplianceWorkOperation.Restore) => ComplianceWorkOperation.CanRestore(context.User, resource),
            nameof(ComplianceWorkOperation.ViewDeleted) => ComplianceWorkOperation.CanManageDeletions(context.User),
            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }
}
