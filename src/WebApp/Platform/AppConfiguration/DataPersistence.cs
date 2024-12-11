using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.Domain.Search;
using AirWeb.EfRepository.DbContext;
using AirWeb.EfRepository.Repositories;
using AirWeb.LocalRepository.Repositories;
using AirWeb.WebApp.Platform.Settings;
using GaEpd.EmailService.EmailLogRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static IServiceCollection AddDataPersistence(this IServiceCollection services,
        ConfigurationManager configuration, IWebHostEnvironment environment)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Use in-memory data for all repositories.
            services
                .AddSingleton<IComplianceSearchRepository, LocalComplianceSearchRepository>()
                .AddSingleton<IEmailLogRepository, LocalEmailLogRepository>()
                .AddSingleton<INotificationTypeRepository, LocalNotificationTypeRepository>()
                .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
                .AddSingleton<IFceRepository, LocalFceRepository>()
                .AddSingleton<IWorkEntryRepository, LocalWorkEntryRepository>();

            return services;
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
                    .ConfigureWarnings(builder => builder.Throw(RelationalEventId.MultipleCollectionIncludeWarning));

                if (environment.IsDevelopment()) dbBuilder.EnableSensitiveDataLogging();
            });
        }

        // Repositories
        services
            .AddScoped<IComplianceSearchRepository, ComplianceSearchRepository>()
            .AddScoped<IEmailLogRepository, EmailLogRepository>()
            .AddScoped<INotificationTypeRepository, NotificationTypeRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>()
            .AddScoped<IFceRepository, FceRepository>()
            .AddScoped<IWorkEntryRepository, WorkEntryRepository>();

        return services;
    }
}
