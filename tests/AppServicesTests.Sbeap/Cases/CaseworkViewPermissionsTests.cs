using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Cases.Permissions;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Sbeap.AppRoles;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Sbeap.Cases;

public class CaseworkViewPermissionsTests
{
    private readonly CaseworkOperation[] _requirements = { CaseworkOperation.ManageDeletions };
    private static CustomerSearchResultDto EmptyCustomer => new();
    private static CaseworkViewDto EmptyCaseworkView => new() { Customer = EmptyCustomer };

    [Test]
    public async Task ManageDeletions_WhenAllowed_Succeeds()
    {
        // Arrange

        // The value for `authenticationType` parameter causes `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Role, SbeapRole.SbeapAdmin)], authenticationType: "Basic"));
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCaseworkView);
        var handler = new CaseworkViewPermissionsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAuthenticated_DoesNotSucceed()
    {
        // Arrange

        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Role, SbeapRole.SbeapAdmin)], authenticationType: null));
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCaseworkView);
        var handler = new CaseworkViewPermissionsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAllowed_DoesNotSucceed()
    {
        // Arrange

        // This `ClaimsPrincipal` is authenticated but does not have the Admin role.
        var user = new ClaimsPrincipal(new ClaimsIdentity(authenticationType: "Basic"));
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCaseworkView);
        var handler = new CaseworkViewPermissionsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}
