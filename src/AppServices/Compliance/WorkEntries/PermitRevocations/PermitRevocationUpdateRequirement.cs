using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;

public class PermitRevocationUpdateRequirement :
    AuthorizationHandler<PermitRevocationUpdateRequirement, PermitRevocationUpdateDto>,
    IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermitRevocationUpdateRequirement requirement,
        PermitRevocationUpdateDto resource)
    {
        if (context.User.IsComplianceStaff() && resource is { IsClosed: false, IsDeleted: false })
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
