using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.EfRepository.DbContext.Configuration;
using GaEpd.EmailService.EmailLogRepository;
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

    // Work entries (mapped to a single table)
    public DbSet<AnnualComplianceCertification> Accs => Set<AnnualComplianceCertification>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<PermitRevocation> PermitRevocations => Set<PermitRevocation>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<RmpInspection> RmpInspections => Set<RmpInspection>();
    public DbSet<SourceTestReview> SourceTestReviews => Set<SourceTestReview>();

    // Enforcement - Case Files
    public DbSet<CaseFile> CaseFiles => Set<CaseFile>();

    // Enforcement - Actions (mapped to a single table)
    public DbSet<AdministrativeOrder> AdministrativeOrders => Set<AdministrativeOrder>();
    public DbSet<ConsentOrder> ConsentOrders => Set<ConsentOrder>();
    public DbSet<OrderResolvedLetter> OrderResolvedLetters => Set<OrderResolvedLetter>();
    public DbSet<InformationalLetter> InformationalLetters => Set<InformationalLetter>();
    public DbSet<LetterOfNoncompliance> LettersOfNoncompliance => Set<LetterOfNoncompliance>();
    public DbSet<NoFurtherActionLetter> NoFurtherActionLetters => Set<NoFurtherActionLetter>();
    public DbSet<NoticeOfViolation> NoticesOfViolation => Set<NoticeOfViolation>();
    public DbSet<NovNfaLetter> NovNfaLetters => Set<NovNfaLetter>();
    public DbSet<ProposedConsentOrder> ProposedConsentOrders => Set<ProposedConsentOrder>();

    // Enforcement - Action properties
    public DbSet<EnforcementActionReview> EnforcementActionReviews => Set<EnforcementActionReview>();
    public DbSet<StipulatedPenalty> StipulatedPenalties => Set<StipulatedPenalty>();

    // Comments (mapped to a single table)
    public DbSet<FceComment> FceComments => Set<FceComment>();
    public DbSet<WorkEntryComment> WorkEntryComments => Set<WorkEntryComment>();
    public DbSet<CaseFileComment> CaseFileComments => Set<CaseFileComment>();

    // Ancillary tables
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Model Builder
        builder
            .ConfigureNavigationAutoIncludes()
            .ConfigureWorkEntryMapping()
            .ConfigureEnforcementActionMapping()
            .ConfigureCommentsMappingStrategy()
            .ConfigureEnumValues()
            .ConfigureCalculatedColumns(Database.ProviderName)
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
