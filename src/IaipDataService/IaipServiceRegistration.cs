using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using IaipDataService.PermitFees;
using IaipDataService.SourceTests;
using IaipDataService.TestData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IaipDataService;

public static class IaipServiceRegistration
{
    public static void AddIaipDataServices(this IHostApplicationBuilder builder, bool connectToIaipDatabase)
    {
        if (connectToIaipDatabase)
        {
            var connectionString = builder.Configuration.GetConnectionString("IaipConnection");
            if (connectionString is null) throw new ArgumentException("IAIP connection string missing.");

            // DB service uses memory cache.
            builder.Services.AddMemoryCache();

            // DB connection factory for Dapper
            builder.Services
                .AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ => new DbConnectionFactory(connectionString))
                .AddSingleton<IFacilityService, IaipFacilityService>()
                .AddSingleton<ISourceTestService, IaipSourceTestService>()
                .AddSingleton<IPermitFeesService, IaipPermitFeesService>();
        }
        else
        {
            builder.Services
                .AddSingleton<IFacilityService, TestFacilityService>()
                .AddSingleton<ISourceTestService, TestSourceTestService>()
                .AddSingleton<IPermitFeesService, TestPermitFeesService>();
        }
    }
}
