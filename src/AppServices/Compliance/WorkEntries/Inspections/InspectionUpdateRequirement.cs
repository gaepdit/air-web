using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public class InspectionUpdateRequirement :
    AuthorizationHandler<InspectionUpdateRequirement, InspectionUpdateDto>,
    IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        InspectionUpdateRequirement requirement,
        InspectionUpdateDto resource)
    {
        if (context.User.IsComplianceStaff() && resource is { IsClosed: false, IsDeleted: false })
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
