using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Core.AuthorizationServices;
using AirWeb.Core.AppRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AppServicesCoreTests.Permissions;

public class RoleBasedPolicy
{
    private IAuthorizationService _authorization = null!;

    [OneTimeSetUp]
    public void SetUp()
    {
        GeneralRole.AddRoles();
        _authorization = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
            collection.AddAuthorizationBuilder().AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer));
    }

    [Test]
    public async Task WhenActiveAndWithRequestedRole_Succeeds()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(AppClaimTypes.ActiveUser, true.ToString()),
            new Claim(ClaimTypes.Role, GeneralRole.SiteMaintenance),
        ], "Basic"));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenWithRequestedRoleButNotActive_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, GeneralRole.SiteMaintenance)], "Basic"));
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
            new Claim(ClaimTypes.Role, GeneralRole.SiteMaintenance),
        ]));
        var result = await _authorization.Succeeded(user, Policies.SiteMaintainer);
        result.Should().BeFalse();
    }
}
