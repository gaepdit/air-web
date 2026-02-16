using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.AppServices.Compliance.Compliance.Permissions;
using AirWeb.Domain.Compliance.AppRoles;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.ComplianceMonitoring.Permissions;

public class ComplianceWorkRequirementsHandlerTests
{
    [Test]
    public async Task ManageDeletions_WhenAllowed_Succeeds()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Delete };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplianceWorkRequirementsHandler(Substitute.For<IComplianceWorkService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAuthenticated_DoesNotSucceed()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Delete };

        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)]));

        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplianceWorkRequirementsHandler(Substitute.For<IComplianceWorkService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAllowed_DoesNotSucceed()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Delete };
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplianceWorkRequirementsHandler(Substitute.For<IComplianceWorkService>());

        // Act
        await handler.HandleAsync(context);

        // Arrange
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task EditWork_WhenDeleted_IsForbidden()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Edit };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new PermitRevocationViewDto { IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplianceWorkRequirementsHandler(Substitute.For<IComplianceWorkService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task EditWork_WhenNotDeleted_IsAllowed()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Edit };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new ComplianceWorkRequirementsHandler(Substitute.For<IComplianceWorkService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }
}
