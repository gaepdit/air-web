using AirWeb.AppServices.Core.EntityServices.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Core.AuthenticationServices;

public static class AuthenticationAppServices
{
    public static IServiceCollection AddAuthenticationAppServices(this IServiceCollection services) => services
        .AddScoped<IClaimsTransformation, AppClaimsTransformation>()
        .AddScoped<IAuthenticationManager, AuthenticationManager>()
        .AddScoped<IUserService, UserService>();
}
