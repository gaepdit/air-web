using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries;

internal class WorkEntrySummaryRequirement :
    AuthorizationHandler<ComplianceWorkOperation, IWorkEntrySummaryDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        IWorkEntrySummaryDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.Close) => ComplianceWorkOperation.CanClose(context.User, resource),
            nameof(ComplianceWorkOperation.Delete) => ComplianceWorkOperation.CanDelete(context.User, resource),
            nameof(ComplianceWorkOperation.Reopen) => ComplianceWorkOperation.CanReopen(context.User, resource),
            nameof(ComplianceWorkOperation.Restore) => ComplianceWorkOperation.CanRestore(context.User, resource),
            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }
}
