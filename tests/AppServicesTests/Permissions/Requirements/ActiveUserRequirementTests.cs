using Microsoft.AspNetCore.Authorization;
using MyApp.AppServices.Permissions.AppClaims;
using MyApp.AppServices.Permissions.Requirements;
using System.Security.Claims;

namespace AppServicesTests.Permissions.Requirements;

public class ActiveUserRequirementTests
{
    [Test]
    public async Task WhenActive_Succeeds()
    {
        var handler = new ActiveUserRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new Claim[] { new(AppClaimTypes.ActiveUser, true.ToString()) }));
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task WhenNotActive_DoesNotSucceed()
    {
        var handler = new ActiveUserRequirement();
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new AuthorizationHandlerContext([handler], user, null);

        await handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }
}
