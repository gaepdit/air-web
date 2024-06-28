using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.EfRepository.DbConnection;
using AirWeb.EfRepository.DbContext;
using AirWeb.EfRepository.Repositories;
using AirWeb.LocalRepository.Repositories;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.EmailService.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static void AddDataPersistence(this IServiceCollection services, ConfigurationManager configuration)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Use in-memory data for all repositories.
            services.AddSingleton<IEmailLogRepository, LocalEmailLogRepository>();
            services.AddSingleton<INotificationTypeRepository, LocalNotificationTypeRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
            services.AddSingleton<IFceRepository, LocalFceRepository>();
            services.AddSingleton<IWorkEntryRepository, LocalWorkEntryRepository>();

            return;
        }

        // When in-memory data is disabled, use a database connection.
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            // In-memory database (not recommended)
            services.AddDbContext<AppDbContext>(builder => builder.UseInMemoryDatabase("TEMP_DB"));
        }
        else
        {
            // Entity Framework context
            services.AddDbContext<AppDbContext>(dbContextOpts =>
            {
                dbContextOpts.UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure());
                dbContextOpts.ConfigureWarnings(builder =>
                    builder.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });

            // Dapper DB connection
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>(_ =>
                new DbConnectionFactory(connectionString));
        }

        // Repositories
        services.AddScoped<IEmailLogRepository, EmailLogRepository>();
        services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IFceRepository, FceRepository>();
        services.AddScoped<IWorkEntryRepository, WorkEntryRepository>();
    }
}
