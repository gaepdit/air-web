using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AppServicesTests.Core.Permissions;

public static class AuthorizationServiceBuilder
{
    public static IAuthorizationService BuildAuthorizationService(Action<IServiceCollection> setupServices)
    {
        var services = new ServiceCollection();
        services.AddAuthorization();
        services.AddLogging();
        setupServices.Invoke(services);
        return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
    }
}
