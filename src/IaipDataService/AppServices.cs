using IaipDataService.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace IaipDataService;

public static class AppServices
{
    public static void AddIaipDataServices(this IServiceCollection services, bool useInMemoryData)
    {
        if (useInMemoryData)
        {
            services.AddSingleton<IFacilityService, LocalFacilityService>();
        }
        else
        {
            services.AddSingleton<IFacilityService, IaipFacilityService>();
        }
    }
}
