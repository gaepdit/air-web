using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Permissions;

internal class CaseFileRequirementsHandler :
    AuthorizationHandler<CaseFileOperation, CaseFileViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseFileOperation requirement,
        CaseFileViewDto resource)
    {
        var user = context.User;
        if (user.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CaseFileOperation.AddComment) => user.CanAddComment(resource),
            nameof(CaseFileOperation.CloseCaseFile) => user.CanClose(resource),
            nameof(CaseFileOperation.DeleteCaseFile) => user.CanDelete(resource),
            nameof(CaseFileOperation.DeleteComment) => user.CanDeleteComment(resource),
            nameof(CaseFileOperation.EditCaseFile) => user.CanEdit(resource),
            nameof(CaseFileOperation.ReopenCaseFile) => user.CanReopen(resource),
            nameof(CaseFileOperation.RestoreCaseFile) => user.CanRestore(resource),
            nameof(CaseFileOperation.View) => user.CanView(resource),
            nameof(CaseFileOperation.ViewDeleted) => user.CanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
