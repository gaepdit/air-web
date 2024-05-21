using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.WorkEntries.BaseCommandDto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AirWeb.AppServices.WorkEntries.Permissions;

public class WorkEntryUpdateRequirement :
    AuthorizationHandler<WorkEntryUpdateRequirement, IWorkEntryUpdateDto>, IAuthorizationRequirement
{
    private ClaimsPrincipal _user = default!;
    private IWorkEntryUpdateDto _resource = default!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkEntryUpdateRequirement requirement,
        IWorkEntryUpdateDto resource)
    {
        _user = context.User;
        _resource = resource;

        if (UserCanEditDetails())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }

    private bool UserCanEditDetails() => IsOpen() && _user.IsManager();
    private bool IsOpen() => _resource is { IsClosed: false, IsDeleted: false };
}
