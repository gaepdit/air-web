using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace IaipDataService;

public static class IaipServiceRegistration
{
    public static void AddIaipDataServices(this IServiceCollection services, bool connectToIaipDatabase,
        string? connectionString)
    {
        if (connectToIaipDatabase)
        {
            // DB service uses memory cache.
            services.AddMemoryCache();

            // DB connection factory for Dapper
            if (connectionString is null) throw new ArgumentException("IAIP connection string missing.");
            services
                .AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ => new DbConnectionFactory(connectionString))
                .AddSingleton<IFacilityService, IaipFacilityService>()
                .AddSingleton<ISourceTestService, IaipSourceTestService>();
        }
        else
        {
            services
                .AddSingleton<IFacilityService, TestFacilityService>()
                .AddSingleton<ISourceTestService, TestSourceTestService>();
        }
    }
}
