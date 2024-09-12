using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public class NotificationUpdateRequirement :
    AuthorizationHandler<NotificationUpdateRequirement, NotificationUpdateDto>,
    IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        NotificationUpdateRequirement requirement,
        NotificationUpdateDto resource)
    {
        if (context.User.IsComplianceStaff() && resource is { IsClosed: false, IsDeleted: false })
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
