using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using Microsoft.Extensions.DependencyInjection;

namespace IaipDataService;

public static class AppServices
{
    public static void AddIaipDataServices(this IServiceCollection services, bool useInMemoryIaipData,
        string? connectionString)
    {
        if (useInMemoryIaipData)
        {
            services.AddSingleton<IFacilityService, LocalFacilityService>();
            services.AddSingleton<ISourceTestService, LocalSourceTestService>();
        }
        else
        {
            // DB connection factory for Dapper
            if (connectionString is null) throw new ArgumentException("IAIP connection string missing.");
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ =>
                new DbConnectionFactory(connectionString));
            services.AddSingleton<IFacilityService, IaipFacilityService>();
            services.AddSingleton<ISourceTestService, IaipSourceTestService>();
        }
    }
}
