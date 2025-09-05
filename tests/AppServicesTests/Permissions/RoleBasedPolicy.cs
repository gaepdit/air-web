using AirWeb.AppServices.AuthenticationServices.Claims;
using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AppServicesTests.Permissions;

public class RoleBasedPolicy
{
    private IAuthorizationService _authorization = null!;

    [SetUp]
    public void SetUp() => _authorization = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
        collection.AddAuthorizationBuilder().AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer));

    [Test]
    public async Task WhenActiveAndWithRequestedRole_Succeeds()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(AppClaimTypes.ActiveUser, true.ToString()),
            new Claim(ClaimTypes.Role, RoleName.SiteMaintenance),
        ], "Basic"));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenWithRequestedRoleButNotActive_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, RoleName.SiteMaintenance)], "Basic"));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenActiveButNotWithRequestedRole_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(AppClaimTypes.ActiveUser, true.ToString())], "Basic"));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenActiveAndWithRequestedRoleButNotAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(AppClaimTypes.ActiveUser, true.ToString()),
            new Claim(ClaimTypes.Role, RoleName.SiteMaintenance),
        ]));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeFalse();
    }
}
