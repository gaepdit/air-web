﻿using AirWeb.AppServices.WorkEntries.Permissions;
using AirWeb.AppServices.WorkEntries.PermitRevocations;
using AirWeb.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AppServicesTests.WorkEntries.Permissions;

public class WorkEntryViewPermissions
{
    [Test]
    public async Task ManageDeletions_WhenAllowed_Succeeds()
    {
        // Arrange
        var requirements = new[]
        {
            WorkEntryOperation.ManageDeletions,
            WorkEntryOperation.ViewDeletedActions,
        };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.Manager) },
            authenticationType: "Basic"));

        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new WorkEntryViewRequirement();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAuthenticated_DoesNotSucceed()
    {
        // Arrange
        var requirements = new[]
        {
            WorkEntryOperation.ManageDeletions,
            WorkEntryOperation.ViewDeletedActions,
        };

        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.Manager) }));

        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new WorkEntryViewRequirement();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAllowed_DoesNotSucceed()
    {
        // Arrange
        var requirements = new[]
        {
            WorkEntryOperation.ManageDeletions,
            WorkEntryOperation.ViewDeletedActions,
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));

        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new WorkEntryViewRequirement();

        // Act
        await handler.HandleAsync(context);

        // Arrange
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task EditEntry_WhenDeleted_IsForbidden()
    {
        // Arrange
        var requirements = new[] { WorkEntryOperation.EditWorkEntry };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.Manager) },
            authenticationType: "Basic"));

        var resource = new PermitRevocationViewDto { IsDeleted = true };
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new WorkEntryViewRequirement();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task EditEntry_WhenNotDeleted_IsAllowed()
    {
        // Arrange
        var requirements = new[] { WorkEntryOperation.EditWorkEntry };

        // The value for the `authenticationType` parameter causes
        // `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(ClaimTypes.Role, RoleName.Manager) },
            authenticationType: "Basic"));

        var resource = new PermitRevocationViewDto();
        var context = new AuthorizationHandlerContext(requirements, user, resource);
        var handler = new WorkEntryViewRequirement();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }
}
