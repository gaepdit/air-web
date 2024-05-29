using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;
using GaEpd.EmailService.Repository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AirWeb.EfRepository.DbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    private const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

    // Domain entities
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<BaseWorkEntry> WorkEntries => Set<BaseWorkEntry>();
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // === Auto-includes ===
        // Some properties should always be included.
        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations

        // Users
        builder.Entity<ApplicationUser>().Navigation(e => e.Office).AutoInclude();

        // Work Entries
        var workEntryEntity = builder.Entity<BaseWorkEntry>();
        workEntryEntity.Navigation(entry => entry.DeletedBy).AutoInclude();

        // === Additional configuration ===

        // Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions
        builder.Entity<BaseWorkEntry>().Property(entry => entry.WorkEntryType).HasConversion<string>();

        // ## The following configurations are Sqlite only. ##
        if (Database.ProviderName != SqliteProvider) return;

#pragma warning disable S125 // Sections of code should not be commented out
        // Sqlite and EF Core are in conflict on how to handle collections of owned types.
        // See: https://stackoverflow.com/a/69826156/212978
        // and: https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
        // builder.Entity<EntityWithOwnedTypeCollection>().OwnsMany(e => e.OwnedTypeCollection, b => b.HasKey("Id"));
        // === UNUSED because there are currently no entities with collections of owned types.
#pragma warning restore S125

        // "Handling DateTimeOffset in SQLite with Entity Framework Core"
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var dateTimeOffsetProperties = entityType.ClrType.GetProperties()
                .Where(info => info.PropertyType == typeof(DateTimeOffset) ||
                               info.PropertyType == typeof(DateTimeOffset?));
            foreach (var property in dateTimeOffsetProperties)
                builder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion(new DateTimeOffsetToBinaryConverter());
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }
}
