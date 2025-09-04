using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Permissions;

internal class CaseFileViewRequirementsHandler :
    AuthorizationHandler<CaseFileOperation, CaseFileViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseFileOperation requirement,
        CaseFileViewDto? resource)
    {
        var user = context.User;
        if (user.Identity is not { IsAuthenticated: true } || resource is null)
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CaseFileOperation.AddComment) => user.CanAddComment(resource),
            nameof(CaseFileOperation.CloseCaseFile) => user.CanCloseCaseFile(resource),
            nameof(CaseFileOperation.DeleteCaseFile) => user.CanDeleteCaseFile(resource),
            nameof(CaseFileOperation.DeleteComment) => user.CanDeleteComment(resource),
            nameof(CaseFileOperation.EditCaseFile) => user.CanEditCaseFile(resource),
            nameof(CaseFileOperation.ReopenCaseFile) => user.CanReopenCaseFile(resource),
            nameof(CaseFileOperation.RestoreCaseFile) => user.CanRestoreCaseFile(resource),
            nameof(CaseFileOperation.View) => user.CanView(resource),
            nameof(CaseFileOperation.ViewDeleted) => user.CanManageDeletions(),
            nameof(CaseFileOperation.ViewDraftEnforcement) => user.CanViewDraftEnforcement(resource),
            _ => false,
        };

        if (success) context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
