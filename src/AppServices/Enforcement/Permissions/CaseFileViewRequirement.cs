using AirWeb.AppServices.Enforcement.Query;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Permissions;

internal class CaseFileViewRequirement(IEnforcementService service) :
    AuthorizationHandler<EnforcementOperation, CaseFileViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EnforcementOperation requirement,
        CaseFileViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(EnforcementOperation.AddComment) => EnforcementOperation.CanAddComment(context.User, resource),
            nameof(EnforcementOperation.Close) => EnforcementOperation.CanClose(context.User, resource),
            nameof(EnforcementOperation.Delete) => EnforcementOperation.CanDelete(context.User, resource),
            nameof(EnforcementOperation.DeleteComment) => EnforcementOperation.CanDeleteComment(context.User, resource),
            nameof(EnforcementOperation.Edit) => EnforcementOperation.CanEdit(context.User, resource),
            nameof(EnforcementOperation.Reopen) => EnforcementOperation.CanReopen(context.User, resource),
            nameof(EnforcementOperation.Restore) => EnforcementOperation.CanRestore(context.User, resource),
            nameof(EnforcementOperation.View) => EnforcementOperation.CanView(context.User, resource),
            nameof(EnforcementOperation.ViewDeleted) => EnforcementOperation.CanManageDeletions(context.User),
            _ => false,
        };

        if (success) context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
