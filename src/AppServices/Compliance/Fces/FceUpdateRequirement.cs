using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces;

public class FceUpdateRequirement :
    AuthorizationHandler<FceUpdateRequirement, FceUpdateDto>,
    IAuthorizationRequirement
{
    private FceUpdateDto _resource = default!;
    private AuthorizationHandlerContext _context = default!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        FceUpdateRequirement requirement,
        FceUpdateDto resource)
    {
        _resource = resource;
        _context = context;

        if (UserCanEditDetails())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }

    private bool UserCanEditDetails() => _context.User.IsComplianceStaff() && !_resource.IsDeleted;
}
