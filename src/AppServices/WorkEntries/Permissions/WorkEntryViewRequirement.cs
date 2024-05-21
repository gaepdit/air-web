using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.WorkEntries.ViewDto;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.WorkEntries.Permissions;

internal class WorkEntryViewRequirement :
    AuthorizationHandler<WorkEntryOperation, IWorkEntryViewDto>
{
    private IWorkEntryViewDto _resource = default!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkEntryOperation requirement,
        IWorkEntryViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        _resource = resource;

        var success = requirement.Name switch
        {
            nameof(WorkEntryOperation.EditWorkEntry) => UserCanEditDetails(),
            nameof(WorkEntryOperation.ManageDeletions) => context.User.IsManager(),
            nameof(WorkEntryOperation.ViewDeletedActions) => context.User.IsManager(),
            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    // Permissions methods
    private bool UserCanEditDetails() => !_resource.IsDeleted;
}
