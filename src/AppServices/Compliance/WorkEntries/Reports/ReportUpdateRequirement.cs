using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public class ReportUpdateRequirement :
    AuthorizationHandler<ReportUpdateRequirement, ReportUpdateDto>,
    IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ReportUpdateRequirement requirement,
        ReportUpdateDto resource)
    {
        if (context.User.IsComplianceStaff() && resource is { IsClosed: false, IsDeleted: false })
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
