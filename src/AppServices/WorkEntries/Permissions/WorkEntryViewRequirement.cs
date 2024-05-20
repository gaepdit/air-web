using Microsoft.AspNetCore.Authorization;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.WorkEntries.ViewDto;
using System.Security.Claims;

namespace AirWeb.AppServices.WorkEntries.Permissions;

internal class WorkEntryViewRequirement :
    AuthorizationHandler<WorkEntryOperation, IWorkEntryViewDto>
{
    private ClaimsPrincipal _user = default!;
    private IWorkEntryViewDto _resource = default!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkEntryOperation requirement,
        IWorkEntryViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        _user = context.User;
        _resource = resource;

        var success = requirement.Name switch
        {
            nameof(WorkEntryOperation.EditWorkEntry) => UserCanEditDetails(),
            nameof(WorkEntryOperation.ManageDeletions) => _user.IsManager(),
            nameof(WorkEntryOperation.ViewDeletedActions) => _user.IsManager(),
            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    // Permissions methods
    private bool UserCanEditDetails() => IsOpen() && _user.IsManager();
    private bool IsOpen() => _resource is { Closed: false, IsDeleted: false };
}
