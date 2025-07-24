using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Permissions;

internal class CaseFileSummaryRequirementsHandler :
    AuthorizationHandler<CaseFileOperation, CaseFileSummaryDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseFileOperation requirement,
        CaseFileSummaryDto? resource)
    {
        var user = context.User;
        if (user.Identity is not { IsAuthenticated: true } || resource is null)
            return Task.FromResult(0);

        var success = requirement.Name == nameof(CaseFileOperation.DeleteComment) &&
                      user.CanDeleteComment(resource);

        if (success) context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
