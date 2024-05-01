﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.AppClaims;
using AirWeb.AppServices.Permissions.Helpers;
using System.Security.Claims;

namespace AppServicesTests.Permissions.PolicyTests;

public class ActiveUserPolicy
{
    private IAuthorizationService _authorization = null!;

    [SetUp]
    public void SetUp() => _authorization = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
        collection.AddAuthorizationBuilder()
            .AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser));

    [Test]
    public async Task WhenActiveAndAuthenticated_Succeeds()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(AppClaimTypes.ActiveUser, true.ToString()), },
            "Basic"));
        var result = await _authorization.Succeeded(user, Policies.ActiveUser);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotActive_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var result = await _authorization.Succeeded(user, Policies.ActiveUser);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(AppClaimTypes.ActiveUser, true.ToString()), }));
        var result = await _authorization.Succeeded(user, Policies.ActiveUser);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNeitherActiveNorAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var result = await _authorization.Succeeded(user, Policies.ActiveUser);
        result.Should().BeFalse();
    }
}
