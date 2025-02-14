using AirWeb.AppServices.Enforcement.CaseFileQuery;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Permissions;

internal class EnforcementRequirementsHandler :
    AuthorizationHandler<EnforcementOperation, CaseFileViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EnforcementOperation requirement,
        CaseFileViewDto resource)
    {
        var user = context.User;
        if (user.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(EnforcementOperation.AddComment) => user.CanAddComment(resource),
            nameof(EnforcementOperation.Close) => user.CanClose(resource),
            nameof(EnforcementOperation.Delete) => user.CanDelete(resource),
            nameof(EnforcementOperation.DeleteComment) => user.CanDeleteComment(resource),
            nameof(EnforcementOperation.Edit) => user.CanEdit(resource),
            nameof(EnforcementOperation.Reopen) => user.CanReopen(resource),
            nameof(EnforcementOperation.Restore) => user.CanRestore(resource),
            nameof(EnforcementOperation.View) => user.CanView(resource),
            nameof(EnforcementOperation.ViewDeleted) => user.CanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
