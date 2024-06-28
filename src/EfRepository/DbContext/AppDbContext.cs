using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
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
    public DbSet<NotificationType> NotificationTypes => Set<NotificationType>();
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<Fce> Fces => Set<Fce>();
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

        // Convert Facility ID to a string for use as primary key.
        builder.Entity<Facility>().Property(facility => facility.Id)
            .HasConversion(facilityId => facilityId.ToString(), s => (FacilityId)s);

        // Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions
        builder.Entity<BaseWorkEntry>().Property(entry => entry.WorkEntryType).HasConversion<string>();

        // === The following configurations are for Sqlite only.
        if (Database.ProviderName != SqliteProvider) return;

        // == Collections of owned types
        // Sqlite and EF Core are in conflict on how to handle collections of owned types.
        // See: https://stackoverflow.com/a/69826156/212978
        // and: https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types

        builder.Entity<Fce>().OwnsMany(fce => fce.Comments, owned =>
        {
            owned.HasKey(propertyNames: "Id");
            owned.Property(comment => comment.CommentedAt).HasConversion(new DateTimeOffsetToBinaryConverter());
        });

        builder.Entity<BaseWorkEntry>().OwnsMany(entry => entry.Comments, owned =>
        {
            owned.HasKey(propertyNames: "Id");
            owned.Property(comment => comment.CommentedAt).HasConversion(new DateTimeOffsetToBinaryConverter());
        });

        // == "Handling DateTimeOffset in SQLite with Entity Framework Core"
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            // This doesn't work with owned types which instead are configured above.
            if (entityType.FindOwnership() != null) continue;

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
