using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Sbeap.Customers.Permissions;

internal class ContactUpdatePermissionsHandler :
    AuthorizationHandler<CustomerOperation, ContactUpdateDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomerOperation requirement,
        ContactUpdateDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CustomerOperation.Edit) =>
                // Contacts can only be edited if they and the associated Customer are not deleted.
                context.User.IsSbeapStaff() && IsNotDeleted(resource),

            nameof(CustomerOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsSbeapAdmin(),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(ContactUpdateDto resource) =>
        resource is { IsDeleted: false, CustomerIsDeleted: false };
}
