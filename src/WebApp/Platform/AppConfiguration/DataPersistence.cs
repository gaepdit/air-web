using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EmailLog;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.EfRepository.Contexts;
using AirWeb.EfRepository.Contexts.SeedDevData;
using AirWeb.EfRepository.Repositories;
using AirWeb.LocalRepository.Repositories;
using AirWeb.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static async Task ConfigureDataPersistence(this IHostApplicationBuilder builder)
    {
        if (AppSettings.DevSettings.UseDevSettings)
        {
            await builder.ConfigureDevDataPersistence();
            return;
        }

        builder.ConfigureDatabaseServices();

        await using var migrationContext = new AppDbContext(GetMigrationDbOpts(builder.Configuration).Options);
        await migrationContext.Database.MigrateAsync();
        await migrationContext.CreateMissingRolesAsync(builder.Services);
    }

    private static void ConfigureDatabaseServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("No connection string found.");

        // Entity Framework context
        builder.Services.AddDbContext<AppDbContext>(db =>
        {
            db.UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure());
            db.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));

            if (builder.Environment.IsDevelopment()) db.EnableSensitiveDataLogging().EnableDetailedErrors();
        });

        // Repositories
        builder.Services
            .AddScoped<IEmailLogRepository, EmailLogRepository>()
            .AddScoped<INotificationTypeRepository, NotificationTypeRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>()
            .AddScoped<IFceRepository, FceRepository>()
            .AddScoped<IWorkEntryRepository, WorkEntryRepository>()

            // TODO: Replace these with EF repositories.
            .AddSingleton<IEnforcementActionRepository, LocalEnforcementActionRepository>()
            .AddSingleton<ICaseFileRepository, LocalCaseFileRepository>();
    }

    private static DbContextOptionsBuilder<AppDbContext> GetMigrationDbOpts(IConfiguration configuration)
    {
        var migConnString = configuration.GetConnectionString("MigrationConnection");
        if (string.IsNullOrEmpty(migConnString))
            throw new InvalidOperationException("No migration connection string found.");

        return new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(migConnString, sqlServerOpts => sqlServerOpts.MigrationsAssembly(nameof(EfRepository)));
    }

    private static async Task CreateMissingRolesAsync(this AppDbContext migrationContext, IServiceCollection services)
    {
        // Initialize any new roles.
        var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in AppRole.AllRoles!.Keys)
            if (!await migrationContext.Roles.AnyAsync(idRole => idRole.Name == role))
                await roleManager.CreateAsync(new IdentityRole(role));
    }

    private static async Task ConfigureDevDataPersistence(this IHostApplicationBuilder builder)
    {
        // When configured, build a SQL Server database; otherwise, use in-memory data.
        if (AppSettings.DevSettings.BuildDatabase)
        {
            builder.ConfigureDatabaseServices();

            await using var migrationContext = new AppDbContext(GetMigrationDbOpts(builder.Configuration).Options);
            await migrationContext.Database.EnsureDeletedAsync();

            if (AppSettings.DevSettings.UseEfMigrations)
                await migrationContext.Database.MigrateAsync();
            else
                await migrationContext.Database.EnsureCreatedAsync();

            DbSeedDataHelpers.SeedAllData(migrationContext);
        }
        else
        {
            builder.Services
                .AddSingleton<IEmailLogRepository, LocalEmailLogRepository>()
                .AddSingleton<INotificationTypeRepository, LocalNotificationTypeRepository>()
                .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
                .AddSingleton<IFceRepository, LocalFceRepository>()
                .AddSingleton<IWorkEntryRepository, LocalWorkEntryRepository>()
                .AddSingleton<IEnforcementActionRepository, LocalEnforcementActionRepository>()
                .AddSingleton<ICaseFileRepository, LocalCaseFileRepository>();
        }
    }
}
