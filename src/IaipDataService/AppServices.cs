using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using Microsoft.Extensions.DependencyInjection;

namespace IaipDataService;

public static class AppServices
{
    public static void AddIaipDataServices(this IServiceCollection services, bool useInMemoryData)
    {
        if (useInMemoryData)
        {
            services.AddSingleton<IFacilityService, LocalFacilityService>();
            services.AddSingleton<ISourceTestService, LocalSourceTestService>();
        }
        else
        {
            services.AddSingleton<IFacilityService, IaipFacilityService>();
            services.AddSingleton<ISourceTestService, IaipSourceTestService>();
        }
    }
}
