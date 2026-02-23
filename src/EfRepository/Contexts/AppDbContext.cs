using AirWeb.Domain.Compliance.AuditPoints;
using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Compliance.EnforcementEntities.ViolationTypes;
using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AirWeb.Domain.Sbeap.Entities.Cases;
using AirWeb.Domain.Sbeap.Entities.Contacts;
using AirWeb.Domain.Sbeap.Entities.Customers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AirWeb.EfRepository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    private const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

    // By default, Entity Framework uses the TPH strategy for modeling inheritance. Inherited types are stored
    // in a single table with a discriminator column.
    //   * Each inherited type listed below is directly available for querying as a DbSet, e.g. `context.Accs`.
    //   * Each base type is also available for querying, e.g. `Context.Set<ComplianceWork>()`.  
    // See: [Inheritance - EF Core | Microsoft Learn](https://learn.microsoft.com/en-us/ef/core/modeling/inheritance)

    // Offices (stored in the `Lookups` table)
    public DbSet<Office> Offices { get; set; } = null!;

    // -- Compliance lookup tables (stored in the `Lookups` table)
    public DbSet<NotificationType> NotificationTypes { get; set; } = null!;
    public DbSet<ViolationType> ViolationTypes { get; set; } = null!;

    // FCEs
    public DbSet<Fce> Fces { get; set; } = null!;

    // Compliance Work (all stored in the `ComplianceWork` table)
    public DbSet<AnnualComplianceCertification> Accs { get; set; } = null!;
    public DbSet<Inspection> Inspections { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<PermitRevocation> PermitRevocations { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;
    public DbSet<RmpInspection> RmpInspections { get; set; } = null!;
    public DbSet<SourceTestReview> SourceTestReviews { get; set; } = null!;

    // Enforcement Case Files
    public DbSet<CaseFile> EnforcementCaseFiles { get; set; } = null!;

    // Enforcement Actions (all stored in the `EnforcementActions` table)
    public DbSet<AdministrativeOrder> AdministrativeOrders { get; set; } = null!;
    public DbSet<ConsentOrder> ConsentOrders { get; set; } = null!;
    public DbSet<InformationalLetter> InformationalLetters { get; set; } = null!;
    public DbSet<LetterOfNoncompliance> LettersOfNoncompliance { get; set; } = null!;
    public DbSet<NoFurtherActionLetter> NoFurtherActionLetters { get; set; } = null!;
    public DbSet<NoticeOfViolation> NoticesOfViolation { get; set; } = null!;
    public DbSet<NovNfaLetter> NovNfaLetters { get; set; } = null!;
    public DbSet<ProposedConsentOrder> ProposedConsentOrders { get; set; } = null!;

    // Enforcement Action property entities
    public DbSet<EnforcementActionReview> EnforcementActionReviews { get; set; } = null!;
    public DbSet<StipulatedPenalty> StipulatedPenalties { get; set; } = null!;

    // Comments (stored in the `Comments` table)
    public DbSet<FceComment> FceComments { get; set; } = null!;
    public DbSet<ComplianceWorkComment> ComplianceWorkComments { get; set; } = null!;
    public DbSet<CaseFileComment> CaseFileComments { get; set; } = null!;

    // Audit Points (stored in the `AuditPoints` table)
    public DbSet<CaseFileAuditPoint> CaseFileAuditPoints { get; set; } = null!;
    public DbSet<FceAuditPoint> FceAuditPoints { get; set; } = null!;
    public DbSet<ComplianceWorkAuditPoint> ComplianceWorkAuditPoints { get; set; } = null!;

    // SBEAP
    public DbSet<ActionItem> SbeapActionItems { get; set; } = null!;
    public DbSet<Casework> SbeapCases { get; set; } = null!;
    public DbSet<Contact> SbeapContacts { get; set; } = null!;
    public DbSet<Customer> SbeapCustomers { get; set; } = null!;

    // -- SBEAP lookup tables (stored in the `Lookups` table)
    public DbSet<ActionItemType> ActionItemTypes { get; set; } = null!;
    public DbSet<Agency> SbeapAgencies { get; set; } = null!;

    // Email Logs
    public DbSet<EmailLog> EmailLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Model Builder
        builder
            .ConfigureNavigationAutoIncludes()
            .ConfigureComplianceWorkTphMapping()
            .ConfigureEnforcementActionTphMapping()
            .ConfigureCommonTphMapping()
            .ConfigureEnumValues()
            .ConfigureImpliedAddedChildEntities()
            .ConfigureLookupTableNameMaxLength()
            .ConfigureCollectionPropertySerialization()
            .ConfigureDataExchangeIndexes();

        // SQLite-only configuration
        if (Database.ProviderName == SqliteProvider)
            builder
                .ConfigureDateTimeOffsetHandling()
                .ConfigureOwnedTypeCollections()
                .ConfigureIdentityPasskeyData();

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
