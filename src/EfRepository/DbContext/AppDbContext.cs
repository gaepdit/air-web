using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.NotificationTypes;
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

    // Maintenance items
    public DbSet<NotificationType> NotificationTypes => Set<NotificationType>();
    public DbSet<Office> Offices => Set<Office>();

    // Domain entities
    public DbSet<Fce> Fces => Set<Fce>();

    // Work entry/compliance event hierarchy
    //   By default, Entity Framework uses the TPH strategy for modeling inheritance. All work entries and compliance
    //   events will be stored in a single table with a discriminator column. Each subtype and each base type are all
    //   available as DbSets for querying.
    //   See: [Inheritance - EF Core | Microsoft Learn](https://learn.microsoft.com/en-us/ef/core/modeling/inheritance)

    // Work entries
    public DbSet<WorkEntry> WorkEntries => Set<WorkEntry>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<PermitRevocation> PermitRevocations => Set<PermitRevocation>();

    // Compliance events
    public DbSet<ComplianceEvent> ComplianceEvents => Set<ComplianceEvent>();
    public DbSet<AnnualComplianceCertification> Accs => Set<AnnualComplianceCertification>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<RmpInspection> RmpInspections => Set<RmpInspection>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<SourceTestReview> SourceTestReviews => Set<SourceTestReview>();

    // Ancillary tables
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
        var workEntryEntity = builder.Entity<WorkEntry>();
        workEntryEntity.Navigation(entry => entry.DeletedBy).AutoInclude();

        // === Additional configuration ===

#pragma warning disable S125
        // // FUTURE: Convert Facility ID to a string for use as primary key.
        // // (When Facility is added as an entity in this DbContext, the following code will be useful.)
        // builder.Entity<Facility>().Property(facility => facility.Id)
        //     .HasConversion(facilityId => facilityId.ToString(), s => (FacilityId)s);
#pragma warning restore S125

        // Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions
        builder.Entity<WorkEntry>().Property(entity => entity.WorkEntryType).HasConversion<string>();
        builder.Entity<ComplianceEvent>().Property(entity => entity.ComplianceEventType).HasConversion<string>();

        // == Collections of owned types
        // Sqlite and EF Core are in conflict on how to handle collections of owned types.
        // See: https://stackoverflow.com/a/69826156/212978
        // and: https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
        builder.Entity<Fce>().OwnsMany(fce => fce.Comments, owned =>
        {
            owned.ToTable("Fce_Comments");
            if (Database.ProviderName != SqliteProvider) return;
            owned.HasKey(propertyNames: "Id");
            owned.Property(comment => comment.CommentedAt).HasConversion(new DateTimeOffsetToBinaryConverter());
        });

        builder.Entity<WorkEntry>().OwnsMany(entry => entry.Comments, owned =>
        {
            owned.ToTable("WorkEntry_Comments");
            if (Database.ProviderName != SqliteProvider) return;
            owned.HasKey(propertyNames: "Id");
            owned.Property(comment => comment.CommentedAt).HasConversion(new DateTimeOffsetToBinaryConverter());
        });

        // == "Handling DateTimeOffset in SQLite with Entity Framework Core"
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        if (Database.ProviderName != SqliteProvider) return;
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
