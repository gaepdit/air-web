using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.Domain.Search;
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
    public static void AddDataPersistence(this IServiceCollection services, ConfigurationManager configuration,
        IWebHostEnvironment environment)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Use in-memory data for all repositories.
            services.AddSingleton<IComplianceSearchRepository, LocalComplianceSearchRepository>();
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
            services.AddDbContext<AppDbContext>(dbBuilder =>
            {
                dbBuilder
                    .UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure())
                    .ConfigureWarnings(warningsBuilder =>
                        warningsBuilder.Throw(RelationalEventId.MultipleCollectionIncludeWarning));

                if (environment.IsDevelopment()) dbBuilder.EnableSensitiveDataLogging();
            });
        }

        // Repositories
        services.AddScoped<IComplianceSearchRepository, ComplianceSearchRepository>();
        services.AddScoped<IEmailLogRepository, EmailLogRepository>();
        services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IFceRepository, FceRepository>();
        services.AddScoped<IWorkEntryRepository, WorkEntryRepository>();
    }
}
