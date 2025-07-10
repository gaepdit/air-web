using AirWeb.AppServices.AuthenticationServices.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.AuthenticationServices;

public static class AuthenticationServices
{
    public static IServiceCollection AddAuthenticationAppServices(this IServiceCollection services) => services
        .AddScoped<IClaimsTransformation, AppClaimsTransformation>()
        .AddScoped<IAuthenticationManager, AuthenticationManager>()
        .AddScoped<IUserService, UserService>();
}
