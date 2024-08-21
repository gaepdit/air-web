using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Permissions;

internal class FceViewRequirement :
    AuthorizationHandler<ComplianceWorkOperation, FceViewDto>
{
    private FceViewDto _resource = null!;
    private AuthorizationHandlerContext _context = null!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        FceViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        _resource = resource;
        _context = context;

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.Edit) => UserCanEditDetails(),
            nameof(ComplianceWorkOperation.ManageDeletions) => UserCanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private bool UserCanEditDetails() => _context.User.IsStaff() && !_resource.IsDeleted;
    private bool UserCanManageDeletions() => _context.User.IsManager();
}
