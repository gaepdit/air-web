using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EmailLog;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.EfRepository.Contexts.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AirWeb.EfRepository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    internal const string SqlServerProvider = "Microsoft.EntityFrameworkCore.SqlServer";
    internal const string SqliteProvider = "Microsoft.EntityFrameworkCore.Sqlite";

    // Maintenance items (These are stored in the `SelectLists` table.)
    public DbSet<NotificationType> NotificationTypes => Set<NotificationType>();
    public DbSet<Office> Offices => Set<Office>();

    // Lookup tables
    public DbSet<ViolationType> ViolationTypes => Set<ViolationType>();

    // FCEs
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

    // Audit Points (mapped to a single table)
    public DbSet<CaseFileAuditPoint> CaseFileAuditPoints => Set<CaseFileAuditPoint>();
    public DbSet<FceAuditPoint> FceAuditPoints => Set<FceAuditPoint>();
    public DbSet<WorkEntryAuditPoint> WorkEntryAuditPoints => Set<WorkEntryAuditPoint>();

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
            .ConfigureInheritanceMapping()
            .ConfigureImpliedAddedChildEntities()
            .ConfigureEnumValues()
            .ConfigureDateTimeOffsetHandling(Database.ProviderName)
            .ConfigureCollectionSerialization()
            .ConfigureCompositeUniqueIndexes();

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await SetActionNumbersAsync(cancellationToken).ConfigureAwait(false);
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public override int SaveChanges()
    {
        SetActionNumbers();
        return base.SaveChanges();
    }

    private async Task SetActionNumbersAsync(CancellationToken cancellationToken)
    {
        // Track assigned action numbers for this transaction to avoid duplicates
        // ActionNumber must be unique across ALL entity types for a given facility
        var assignedNumbers = new Dictionary<string, ushort>();

        // Get all added entities that need ActionNumber assignment
        var addedFces = ChangeTracker.Entries<Fce>()
            .Where(e => e.State == EntityState.Added && e.Entity.ActionNumber == null)
            .ToList();

        var addedCaseFiles = ChangeTracker.Entries<CaseFile>()
            .Where(e => e.State == EntityState.Added && e.Entity.ActionNumber == null)
            .ToList();

        var addedComplianceEvents = ChangeTracker.Entries<ComplianceEvent>()
            .Where(e => e.State == EntityState.Added && e.Entity.ActionNumber == null)
            .ToList();

        // Helper method to get the next action number for a facility across all entity types
        async Task<ushort> GetNextActionNumberAsync(string facilityId)
        {
            if (assignedNumbers.ContainsKey(facilityId))
                return assignedNumbers[facilityId]++;

            // Query max ActionNumber across ALL entity types for this facility
            var maxFce = await Fces.AsNoTracking()
                .Where(f => f.FacilityId == facilityId && f.ActionNumber != null)
                .Select(f => f.ActionNumber)
                .MaxAsync(cancellationToken)
                .ConfigureAwait(false);

            var maxCaseFile = await CaseFiles.AsNoTracking()
                .Where(c => c.FacilityId == facilityId && c.ActionNumber != null)
                .Select(c => c.ActionNumber)
                .MaxAsync(cancellationToken)
                .ConfigureAwait(false);

            var maxComplianceEvent = await Set<ComplianceEvent>().AsNoTracking()
                .Where(w => w.FacilityId == facilityId && w.ActionNumber != null)
                .Select(w => w.ActionNumber)
                .MaxAsync(cancellationToken)
                .ConfigureAwait(false);

            // Find the overall maximum across all entity types
            var overallMax = new[] { maxFce ?? 0, maxCaseFile ?? 0, maxComplianceEvent ?? 0 }.Max();
            var nextNumber = overallMax + 1;

            if (nextNumber > ushort.MaxValue)
                throw new InvalidOperationException(
                    $"Action number limit exceeded for facility {facilityId}. Maximum value is {ushort.MaxValue}.");

            var result = (ushort)nextNumber;
            assignedNumbers[facilityId] = (ushort)(result + 1);
            return result;
        }

        // Assign ActionNumbers for FCEs
        foreach (var entry in addedFces)
        {
            var facilityId = entry.Entity.FacilityId;
            var actionNumber = await GetNextActionNumberAsync(facilityId);
            entry.Property(nameof(Fce.ActionNumber)).CurrentValue = actionNumber;
        }

        // Assign ActionNumbers for Case Files
        foreach (var entry in addedCaseFiles)
        {
            var facilityId = entry.Entity.FacilityId;
            var actionNumber = await GetNextActionNumberAsync(facilityId);
            entry.Property(nameof(CaseFile.ActionNumber)).CurrentValue = actionNumber;
        }

        // Assign ActionNumbers for Compliance Events (WorkEntries)
        foreach (var entry in addedComplianceEvents)
        {
            var facilityId = entry.Entity.FacilityId;
            var actionNumber = await GetNextActionNumberAsync(facilityId);
            entry.Property(nameof(ComplianceEvent.ActionNumber)).CurrentValue = actionNumber;
        }
    }

    private void SetActionNumbers()
    {
        // Synchronous version for SaveChanges()
        // Track assigned action numbers for this transaction to avoid duplicates
        // ActionNumber must be unique across ALL entity types for a given facility
        var assignedNumbers = new Dictionary<string, ushort>();

        // Get all added entities that need ActionNumber assignment
        var addedFces = ChangeTracker.Entries<Fce>()
            .Where(e => e.State == EntityState.Added && e.Entity.ActionNumber == null)
            .ToList();

        var addedCaseFiles = ChangeTracker.Entries<CaseFile>()
            .Where(e => e.State == EntityState.Added && e.Entity.ActionNumber == null)
            .ToList();

        var addedComplianceEvents = ChangeTracker.Entries<ComplianceEvent>()
            .Where(e => e.State == EntityState.Added && e.Entity.ActionNumber == null)
            .ToList();

        // Helper method to get the next action number for a facility across all entity types
        ushort GetNextActionNumber(string facilityId)
        {
            if (assignedNumbers.ContainsKey(facilityId))
                return assignedNumbers[facilityId]++;

            // Query max ActionNumber across ALL entity types for this facility
            var maxFce = Fces.AsNoTracking()
                .Where(f => f.FacilityId == facilityId && f.ActionNumber != null)
                .Select(f => f.ActionNumber)
                .Max();

            var maxCaseFile = CaseFiles.AsNoTracking()
                .Where(c => c.FacilityId == facilityId && c.ActionNumber != null)
                .Select(c => c.ActionNumber)
                .Max();

            var maxComplianceEvent = Set<ComplianceEvent>().AsNoTracking()
                .Where(w => w.FacilityId == facilityId && w.ActionNumber != null)
                .Select(w => w.ActionNumber)
                .Max();

            // Find the overall maximum across all entity types
            var overallMax = new[] { maxFce ?? 0, maxCaseFile ?? 0, maxComplianceEvent ?? 0 }.Max();
            var nextNumber = overallMax + 1;

            if (nextNumber > ushort.MaxValue)
                throw new InvalidOperationException(
                    $"Action number limit exceeded for facility {facilityId}. Maximum value is {ushort.MaxValue}.");

            var result = (ushort)nextNumber;
            assignedNumbers[facilityId] = (ushort)(result + 1);
            return result;
        }

        // Assign ActionNumbers for FCEs
        foreach (var entry in addedFces)
        {
            var facilityId = entry.Entity.FacilityId;
            var actionNumber = GetNextActionNumber(facilityId);
            entry.Property(nameof(Fce.ActionNumber)).CurrentValue = actionNumber;
        }

        // Assign ActionNumbers for Case Files
        foreach (var entry in addedCaseFiles)
        {
            var facilityId = entry.Entity.FacilityId;
            var actionNumber = GetNextActionNumber(facilityId);
            entry.Property(nameof(CaseFile.ActionNumber)).CurrentValue = actionNumber;
        }

        // Assign ActionNumbers for Compliance Events (WorkEntries)
        foreach (var entry in addedComplianceEvents)
        {
            var facilityId = entry.Entity.FacilityId;
            var actionNumber = GetNextActionNumber(facilityId);
            entry.Property(nameof(ComplianceEvent.ActionNumber)).CurrentValue = actionNumber;
        }
    }
}
