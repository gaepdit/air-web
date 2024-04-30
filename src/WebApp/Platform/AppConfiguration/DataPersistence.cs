using GaEpd.EmailService.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyApp.Domain.Entities.EntryActions;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.Offices;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.EfRepository.DbConnection;
using MyApp.EfRepository.DbContext;
using MyApp.EfRepository.Repositories;
using MyApp.LocalRepository.Repositories;
using MyApp.WebApp.Platform.Settings;

namespace MyApp.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static void AddDataPersistence(this IServiceCollection services, ConfigurationManager configuration)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            // Use in-memory data for all repositories.
            services.AddSingleton<IEmailLogRepository, LocalEmailLogRepository>();
            services.AddSingleton<IEntryActionRepository, LocalEntryActionRepository>();
            services.AddSingleton<IEntryTypeRepository,LocalEntryTypeRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
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
        services.AddScoped<IEntryActionRepository, EntryActionRepository>();
        services.AddScoped<IEntryTypeRepository,EntryTypeRepository>();
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IWorkEntryRepository, WorkEntryRepository>();
    }
}
