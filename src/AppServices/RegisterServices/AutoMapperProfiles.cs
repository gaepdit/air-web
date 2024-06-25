using AirWeb.AppServices.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.RegisterServices;

public static class AutoMapperProfiles
{
    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        // Add AutoMapper profiles
        services.AddAutoMapper(configuration => configuration.AddProfile<AutoMapperProfile>());
    }
}
