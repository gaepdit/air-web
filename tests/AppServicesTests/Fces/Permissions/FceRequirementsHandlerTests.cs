using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.Domain.AppRoles;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Fces.Permissions;

public class FceRequirementsHandlerTests
{
    private readonly FacilityId _facilityId = new("00100001");

    [Test]
    public async Task ComplianceStaff_CanEdit()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Edit };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, ComplianceRole.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceRequirementsHandler(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task GivenADeletedFce_CannotEdit()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Edit };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, ComplianceRole.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceRequirementsHandler(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ComplianceManager_CanDelete()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Delete };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceRequirementsHandler(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task NotComplianceManager_CannotDelete()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Delete };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, ComplianceRole.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceRequirementsHandler(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task IfAlreadyDeleted_CannotDelete()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Delete };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceRequirementsHandler(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ComplianceManager_CanRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { FacilityId = _facilityId, IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);

        var fceService = Substitute.For<IFceService>();
        fceService.ExistsAsync(Arg.Any<FacilityId>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new FceRequirementsHandler(fceService);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task IfReplacementExists_CannotRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { FacilityId = _facilityId, IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);

        var fceService = Substitute.For<IFceService>();
        fceService.ExistsAsync(Arg.Any<FacilityId>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var handler = new FceRequirementsHandler(fceService);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task NotComplianceManager_CanNotRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, ComplianceRole.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceRequirementsHandler(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task IfNotDeleted_CannotRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, ComplianceRole.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceRequirementsHandler(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}
