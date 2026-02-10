using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.EmailLog;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.Domain.Identity;
using AirWeb.Domain.Lookups.NotificationTypes;
using AirWeb.Domain.Lookups.Offices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AirWeb.EfRepository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    internal const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

    // Maintenance items (These are stored in the `Lookups` table.)
    public DbSet<NotificationType> NotificationTypes { get; set; } = null!;
    public DbSet<Office> Offices { get; set; } = null!;

    // Lookup tables
    public DbSet<ViolationType> ViolationTypes { get; set; } = null!;

    // FCEs
    public DbSet<Fce> Fces { get; set; } = null!;

    // Compliance work/compliance events
    //   By default, Entity Framework uses the TPH strategy for modeling inheritance. All compliance work and compliance
    //   events will be stored in a single table with a discriminator column. Each subtype and each base type are all
    //   available as DbSets for querying.
    //   See: [Inheritance - EF Core | Microsoft Learn](https://learn.microsoft.com/en-us/ef/core/modeling/inheritance)

    // Compliance work (mapped to a single table)
    public DbSet<AnnualComplianceCertification> Accs { get; set; } = null!;
    public DbSet<Inspection> Inspections { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<PermitRevocation> PermitRevocations { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;
    public DbSet<RmpInspection> RmpInspections { get; set; } = null!;
    public DbSet<SourceTestReview> SourceTestReviews { get; set; } = null!;

    // Enforcement - Case Files
    public DbSet<CaseFile> CaseFiles { get; set; } = null!;

    // Enforcement - Actions (mapped to a single table)
    public DbSet<AdministrativeOrder> AdministrativeOrders { get; set; } = null!;
    public DbSet<ConsentOrder> ConsentOrders { get; set; } = null!;
    public DbSet<InformationalLetter> InformationalLetters { get; set; } = null!;
    public DbSet<LetterOfNoncompliance> LettersOfNoncompliance { get; set; } = null!;
    public DbSet<NoFurtherActionLetter> NoFurtherActionLetters { get; set; } = null!;
    public DbSet<NoticeOfViolation> NoticesOfViolation { get; set; } = null!;
    public DbSet<NovNfaLetter> NovNfaLetters { get; set; } = null!;
    public DbSet<ProposedConsentOrder> ProposedConsentOrders { get; set; } = null!;

    // Enforcement - Action properties
    public DbSet<EnforcementActionReview> EnforcementActionReviews { get; set; } = null!;
    public DbSet<StipulatedPenalty> StipulatedPenalties { get; set; } = null!;

    // Comments (mapped to a single table)
    public DbSet<FceComment> FceComments { get; set; } = null!;
    public DbSet<ComplianceWorkComment> ComplianceWorkComments { get; set; } = null!;
    public DbSet<CaseFileComment> CaseFileComments { get; set; } = null!;

    // Audit Points (mapped to a single table)
    public DbSet<CaseFileAuditPoint> CaseFileAuditPoints { get; set; } = null!;
    public DbSet<FceAuditPoint> FceAuditPoints { get; set; } = null!;
    public DbSet<ComplianceWorkAuditPoint> ComplianceWorkAuditPoints { get; set; } = null!;

    // Ancillary tables
    public DbSet<EmailLog> EmailLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Model Builder
        builder
            .ConfigureNavigationAutoIncludes()
            .ConfigureComplianceWorkMapping()
            .ConfigureEnforcementActionMapping()
            .ConfigureInheritanceMapping()
            .ConfigureImpliedAddedChildEntities()
            .ConfigureEnumValues()
            .ConfigureDateTimeOffsetHandling(Database.ProviderName)
            .ConfigureCollectionSerialization()
            .ConfigureDataExchangeIndexes()
            .ConfigureIdentityPasskeyData(Database.ProviderName);

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
