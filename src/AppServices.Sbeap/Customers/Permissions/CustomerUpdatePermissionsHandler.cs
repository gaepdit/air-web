using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Sbeap.Customers.Permissions;

internal class CustomerUpdatePermissionsHandler :
    AuthorizationHandler<CustomerOperation, CustomerUpdateDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomerOperation requirement,
        CustomerUpdateDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CustomerOperation.Edit) =>
                // Customers can only be edited if they are not deleted.
                context.User.IsSbeapStaff() && IsNotDeleted(resource),

            nameof(CustomerOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsSbeapAdmin(),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(CustomerUpdateDto resource) => !resource.IsDeleted;
}
