using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Identity;
using MyApp.EfRepository.DbContext;
using MyApp.EfRepository.DbContext.DevData;
using MyApp.WebApp.Platform.Settings;

namespace MyApp.WebApp.Platform.AppConfiguration;

public class MigratorHostedService(IServiceProvider serviceProvider, IConfiguration configuration) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        // If using in-memory data store, no further action required.
        if (AppSettings.DevSettings.UseInMemoryData) return;

        var migrationConnectionString = configuration.GetConnectionString("MigrationConnection");
        var migrationOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(migrationConnectionString, builder => builder.MigrationsAssembly("EfRepository")).Options;

        await using var migrationContext = new AppDbContext(migrationOptions);

        if (AppSettings.DevSettings.UseEfMigrations)
        {
            // Run any EF database migrations if used.
            await migrationContext.Database.MigrateAsync(cancellationToken);

            // Initialize any new roles. (No other data is seeded when running EF migrations.)
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in AppRole.AllRoles.Keys)
            {
                if (!await migrationContext.Roles.AnyAsync(identityRole => identityRole.Name == role,
                        cancellationToken))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        else if (AppSettings.DevSettings.DeleteAndRebuildDatabase)
        {
            // Delete and re-create the database.
            await migrationContext.Database.EnsureDeletedAsync(cancellationToken);
            await migrationContext.Database.EnsureCreatedAsync(cancellationToken);

            // Add seed data to database.
            DbSeedDataHelpers.SeedAllData(migrationContext);
        }
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
