﻿using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.Enforcement.Permissions;

public class CaseFileViewPermissionsTests
{
    [Test]
    public async Task ComplianceStaff_CanEdit()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.EditCaseFile };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new CaseFileViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileViewRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task GivenNotComplianceStaff_CannotEdit()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.EditCaseFile };
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(AppClaimTypes.ActiveUser, true.ToString())],
            "Basic"));

        var resource = new CaseFileViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileViewRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task GivenADeletedCaseFile_CannotEdit()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.EditCaseFile };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new CaseFileViewDto { IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileViewRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task GivenAClosedCaseFile_CannotEdit()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.EditCaseFile };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        var resource = new CaseFileViewDto { IsClosed = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileViewRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task GivenResourceIsNull_CannotEdit()
    {
        // Arrange
        var requirements = new[] { CaseFileOperation.EditCaseFile };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, RoleName.ComplianceStaff)],
            authenticationType: "Basic"));

        CaseFileViewDto? resource = null;
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new CaseFileViewRequirementsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}
