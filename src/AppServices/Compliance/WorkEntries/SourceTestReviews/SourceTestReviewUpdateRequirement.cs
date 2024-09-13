using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public class SourceTestReviewUpdateRequirement :
    AuthorizationHandler<SourceTestReviewUpdateRequirement, SourceTestReviewUpdateDto>,
    IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SourceTestReviewUpdateRequirement requirement,
        SourceTestReviewUpdateDto resource)
    {
        if (context.User.IsComplianceStaff() && resource is { IsClosed: false, IsDeleted: false })
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
