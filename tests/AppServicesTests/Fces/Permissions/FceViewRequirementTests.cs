using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Fces.Permissions;

public class FceViewRequirementTests
{
    private readonly FacilityViewDto _facilityViewDto = new()
        { Id = new FacilityId("00100001"), CompanyName = "Company", Description = "Description" };

    [Test]
    public async Task ComplianceStaff_CanEdit()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Edit };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceViewRequirement(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task GivenADeletedFce_CannotEdit()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Edit };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto, IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceViewRequirement(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ComplianceManager_CanDelete()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Delete };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceViewRequirement(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task NotComplianceManager_CannotDelete()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Delete };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceViewRequirement(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task IfAlreadyDeleted_CannotDelete()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Delete };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto, IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceViewRequirement(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ComplianceManager_CanRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto, IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);

        var fceService = Substitute.For<IFceService>();
        fceService.ExistsAsync(Arg.Any<FceRestoreDto>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var handler = new FceViewRequirement(fceService);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task IfReplacementExists_CannotRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto, IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);

        var fceService = Substitute.For<IFceService>();
        fceService.ExistsAsync(Arg.Any<FceRestoreDto>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var handler = new FceViewRequirement(fceService);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task NotComplianceManager_CanNotRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto, IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceViewRequirement(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task IfNotDeleted_CannotRestore()
    {
        // Arrange
        var requirements = new[] { ComplianceWorkOperation.Restore };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new FceViewDto { Facility = _facilityViewDto };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new FceViewRequirement(Substitute.For<IFceService>());

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}
