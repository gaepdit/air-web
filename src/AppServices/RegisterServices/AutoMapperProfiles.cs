using AirWeb.AppServices.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.RegisterServices;

public static class AutoMapperProfiles
{
    // Add AutoMapper profiles
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services) =>
        services.AddAutoMapper(configuration => configuration.AddProfile<AutoMapperProfile>());
}
