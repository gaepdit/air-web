using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Enforcement.Permissions;

public class CaseFileSummaryPermissionsTests
{
    [Test]
    public async Task ComplianceManager_CanDeleteComments()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.DeleteComment };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new CaseFileSummaryDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileSummaryRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task GivenNotComplianceManager_CannotDeleteComments()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.DeleteComment };
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(AppClaimTypes.ActiveUser, true.ToString())],
            "Basic"));

        var resource = new CaseFileSummaryDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileSummaryRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    private const string NameIdentifierId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

    [Test]
    public async Task GivenCaseFileOwner_CanDeleteComments()
    {
        // Arrange
        const string userId = "testUserId";
        var requirements = new[] { CaseFileOperation.DeleteComment };
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(AppClaimTypes.ActiveUser, true.ToString()),
                new Claim(NameIdentifierId, userId),
            ],
            "Basic"));

        var resource = new CaseFileSummaryDto { ResponsibleStaff = new StaffViewDto { Id = userId } };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileSummaryRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task GivenADeletedCaseFile_CannotDeleteComments()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.DeleteComment };
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new CaseFileSummaryDto { IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileSummaryRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task GivenAClosedCaseFile_CanDeleteComments()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.DeleteComment };
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        var resource = new CaseFileSummaryDto { IsClosed = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileSummaryRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task GivenResourceIsNull_CannotDeleteComments()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.DeleteComment };
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceManager)],
            authenticationType: "Basic"));

        CaseFileSummaryDto? resource = null;
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileSummaryRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}
