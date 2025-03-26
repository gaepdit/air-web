using AirWeb.AppServices.Permissions.ComplianceStaff.UserRequirements;
using AirWeb.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Permissions.Requirements;

public class RoleBasedRequirementTests
{
    [Test]
    public async Task WhenRequiredRoleExists_Succeeds()
    {
        var handler = new ComplianceSiteMaintenanceRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Role, RoleName.ComplianceSiteMaintenance),
        ]));
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task WhenRequiredRoleDoesNotExist_Fails()
    {
        var handler = new ComplianceSiteMaintenanceRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Role, RoleName.SiteMaintenance),
        ]));
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task WhenNoRoles_DoesNotSucceed()
    {
        var handler = new ComplianceSiteMaintenanceRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
