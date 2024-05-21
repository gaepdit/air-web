using AirWeb.AppServices.WorkEntries.BaseCommandDto;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.WorkEntries.Permissions;

public class WorkEntryUpdateRequirement :
    AuthorizationHandler<WorkEntryUpdateRequirement, IWorkEntryUpdateDto>, IAuthorizationRequirement
{
    private IWorkEntryUpdateDto _resource = default!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkEntryUpdateRequirement requirement,
        IWorkEntryUpdateDto resource)
    {
        _resource = resource;

        if (UserCanEditDetails())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }

    private bool UserCanEditDetails() => !_resource.IsDeleted;
}
