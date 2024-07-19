using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;
using AirWeb.EfRepository.DbContext.Configuration;
using GaEpd.EmailService.Repository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AirWeb.EfRepository.DbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    internal const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

    // Maintenance items
    public DbSet<NotificationType> NotificationTypes => Set<NotificationType>();
    public DbSet<Office> Offices => Set<Office>();

    // Domain entities
    public DbSet<Fce> Fces => Set<Fce>();

    // Work entries/compliance events
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

        // Configure Model Builder
        builder
            .ConfigureNavigationAutoIncludes()
            .ConfigureTphColumnSharing()
            .ConfigureEnumValues()
            .ConfigureOwnedTypeCollections(Database.ProviderName)
            .ConfigureDateTimeOffsetHandling(Database.ProviderName);

#pragma warning disable S125
        // // FUTURE: == Convert Facility ID to a string for use as primary key.
        // // (When Facility is added as an entity in this DbContext, the following code will be useful.)
        // builder.Entity<Facility>().Property(facility => facility.Id)
        //     .HasConversion(facilityId => facilityId.ToString(), s => (FacilityId)s);
#pragma warning restore S125
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }
}
