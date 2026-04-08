using AirWeb.AppServices.Sbeap.AuthorizationPolicies;
using AirWeb.Domain.Sbeap.AppRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AppServicesTests.Sbeap.Permissions;

public class RoleBasedPolicyTests
{
    private IAuthorizationService _authorizationService = null!;

    [SetUp]
    public void SetUp() => _authorizationService = AuthorizationServiceBuilder.BuildAuthorizationService(collection =>
        collection.AddAuthorizationBuilder()
            .AddPolicy(nameof(SbeapPolicies.SbeapSiteMaintainer), SbeapPolicies.SbeapSiteMaintainer));

    [Test]
    public async Task WhenAuthenticatedAndActiveAndDivisionManager_Succeeds()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(nameof(SbeapPolicies.ActiveUser), true.ToString()),
            new Claim(ClaimTypes.Role, SbeapRole.SbeapSiteMaintenance),
        ], "Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, SbeapPolicies.SbeapSiteMaintainer)).Succeeded;
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotActive_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Role, SbeapRole.SbeapSiteMaintenance),
        ], "Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, SbeapPolicies.SbeapSiteMaintainer)).Succeeded;
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotDivisionManager_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(nameof(SbeapPolicies.ActiveUser), true.ToString()),
        ], "Basic"));
        var result = (await _authorizationService.AuthorizeAsync(user, SbeapPolicies.SbeapSiteMaintainer)).Succeeded;
        result.Should().BeFalse();
    }

    [Test]
    public async Task WhenNotAuthenticated_DoesNotSucceed()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(nameof(SbeapPolicies.ActiveUser), true.ToString()),
            new Claim(ClaimTypes.Role, SbeapRole.SbeapSiteMaintenance),
        ]));
        var result = (await _authorizationService.AuthorizeAsync(user, SbeapPolicies.SbeapSiteMaintainer)).Succeeded;
        result.Should().BeFalse();
    }
}
