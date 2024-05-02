using Microsoft.AspNetCore.Authorization;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.WorkEntries.CommandDto;
using System.Security.Claims;

namespace AirWeb.AppServices.WorkEntries.Permissions;

public class WorkEntryUpdateRequirements :
    AuthorizationHandler<WorkEntryUpdateRequirements, WorkEntryUpdateDto>, IAuthorizationRequirement
{
    private ClaimsPrincipal _user = default!;
    private WorkEntryUpdateDto _resource = default!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkEntryUpdateRequirements requirements,
        WorkEntryUpdateDto resource)
    {
        _user = context.User;
        _resource = resource;

        if (UserCanEditDetails())
            context.Succeed(requirements);

        return Task.FromResult(0);
    }

    private bool UserCanEditDetails() => IsOpen() && _user.IsManager();
    private bool IsOpen() => _resource is { WorkEntryClosed: false, IsDeleted: false };
}
