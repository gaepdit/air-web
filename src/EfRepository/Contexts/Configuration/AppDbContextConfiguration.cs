using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using GaEpd.AppLibrary.Domain.Entities;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace AirWeb.EfRepository.Contexts.Configuration;

internal static class AppDbContextConfiguration
{
    internal static ModelBuilder ConfigureNavigationAutoIncludes(this ModelBuilder builder)
    {
        // Some properties should always be included.
        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations

        // Named entities should always be included.
        builder.Entity<ApplicationUser>().Navigation(user => user.Office).AutoInclude();
        builder.Entity<Notification>().Navigation(notification => notification.NotificationType).AutoInclude();

        // User data should be included in all entities.

        // FCEs
        var fceEntity = builder.Entity<Fce>();
        fceEntity.Navigation(fce => fce.DeletedBy).AutoInclude();
        fceEntity.Navigation(fce => fce.ReviewedBy).AutoInclude();

        // Work Entries
        var workEntryEntity = builder.Entity<WorkEntry>();
        workEntryEntity.Navigation(entry => entry.ClosedBy).AutoInclude();
        workEntryEntity.Navigation(entry => entry.DeletedBy).AutoInclude();
        workEntryEntity.Navigation(entry => entry.ResponsibleStaff).AutoInclude();

        // Enforcement entities
        var caseFileEntity = builder.Entity<CaseFile>();
        caseFileEntity.Navigation(enforcementCase => enforcementCase.ResponsibleStaff).AutoInclude();

        var enforcementActionEntity = builder.Entity<EnforcementAction>();
        enforcementActionEntity.Navigation(enforcementAction => enforcementAction.ApprovedBy).AutoInclude();

        var enforcementActionReviewEntity = builder.Entity<EnforcementActionReview>();
        enforcementActionReviewEntity.Navigation(review => review.ReviewedBy).AutoInclude();

        // Comments
        builder.Entity<Comment>().Navigation(comment => comment.CommentBy).AutoInclude();

        // Audit Points
        builder.Entity<AuditPoint>().Navigation(comment => comment.Who).AutoInclude();

        return builder;
    }

    internal static ModelBuilder ConfigureWorkEntryMapping(this ModelBuilder builder)
    {
        // Work Entries use Table Per Hierarchy (TPH) mapping strategy.
        builder.Entity<WorkEntry>()
            .ToTable("WorkEntries")
            .HasDiscriminator(entry => entry.WorkEntryType)
            .HasValue<AnnualComplianceCertification>(WorkEntryType.AnnualComplianceCertification)
            .HasValue<Inspection>(WorkEntryType.Inspection)
            .HasValue<Notification>(WorkEntryType.Notification)
            .HasValue<PermitRevocation>(WorkEntryType.PermitRevocation)
            .HasValue<Report>(WorkEntryType.Report)
            .HasValue<RmpInspection>(WorkEntryType.RmpInspection)
            .HasValue<SourceTestReview>(WorkEntryType.SourceTestReview);

        // TPH column sharing https://learn.microsoft.com/en-us/ef/core/modeling/inheritance#shared-columns
        var accEntity = builder.Entity<AnnualComplianceCertification>();
        var insEntity = builder.Entity<Inspection>();
        var notEntity = builder.Entity<Notification>();
        var repEntity = builder.Entity<Report>();
        var revEntity = builder.Entity<PermitRevocation>();
        var rmpEntity = builder.Entity<RmpInspection>();
        var strEntity = builder.Entity<SourceTestReview>();

        // ReceivedDate
        accEntity.Property(e => e.ReceivedDate).HasColumnName(nameof(AnnualComplianceCertification.ReceivedDate));
        notEntity.Property(e => e.ReceivedDate).HasColumnName(nameof(Notification.ReceivedDate));
        repEntity.Property(e => e.ReceivedDate).HasColumnName(nameof(Report.ReceivedDate));
        revEntity.Property(e => e.ReceivedDate).HasColumnName(nameof(PermitRevocation.ReceivedDate));

        // EnforcementNeeded
        accEntity.Property(e => e.EnforcementNeeded)
            .HasColumnName(nameof(AnnualComplianceCertification.EnforcementNeeded));
        repEntity.Property(e => e.EnforcementNeeded).HasColumnName(nameof(Report.EnforcementNeeded));

        // Inspections
        insEntity.Property(e => e.InspectionReason).HasColumnName(nameof(Inspection.InspectionReason));
        rmpEntity.Property(e => e.InspectionReason).HasColumnName(nameof(RmpInspection.InspectionReason));
        insEntity.Property(e => e.InspectionStarted).HasColumnName(nameof(Inspection.InspectionStarted));
        rmpEntity.Property(e => e.InspectionStarted).HasColumnName(nameof(RmpInspection.InspectionStarted));
        insEntity.Property(e => e.InspectionEnded).HasColumnName(nameof(Inspection.InspectionEnded));
        rmpEntity.Property(e => e.InspectionEnded).HasColumnName(nameof(RmpInspection.InspectionEnded));
        insEntity.Property(e => e.WeatherConditions).HasColumnName(nameof(Inspection.WeatherConditions));
        rmpEntity.Property(e => e.WeatherConditions).HasColumnName(nameof(RmpInspection.WeatherConditions));
        insEntity.Property(e => e.InspectionGuide).HasColumnName(nameof(Inspection.InspectionGuide));
        rmpEntity.Property(e => e.InspectionGuide).HasColumnName(nameof(RmpInspection.InspectionGuide));
        insEntity.Property(e => e.FacilityOperating).HasColumnName(nameof(Inspection.FacilityOperating));
        rmpEntity.Property(e => e.FacilityOperating).HasColumnName(nameof(RmpInspection.FacilityOperating));
        insEntity.Property(e => e.DeviationsNoted).HasColumnName(nameof(Inspection.DeviationsNoted));
        rmpEntity.Property(e => e.DeviationsNoted).HasColumnName(nameof(RmpInspection.DeviationsNoted));

        // FollowupTaken
        insEntity.Property(e => e.FollowupTaken).HasColumnName(nameof(Inspection.FollowupTaken));
        notEntity.Property(e => e.FollowupTaken).HasColumnName(nameof(Notification.FollowupTaken));
        revEntity.Property(e => e.FollowupTaken).HasColumnName(nameof(PermitRevocation.FollowupTaken));
        rmpEntity.Property(e => e.FollowupTaken).HasColumnName(nameof(RmpInspection.FollowupTaken));
        strEntity.Property(e => e.FollowupTaken).HasColumnName(nameof(SourceTestReview.FollowupTaken));

        // DueDate
        notEntity.Property(e => e.DueDate).HasColumnName(nameof(Notification.DueDate));
        repEntity.Property(e => e.DueDate).HasColumnName(nameof(Report.DueDate));
        strEntity.Property(e => e.DueDate).HasColumnName(nameof(SourceTestReview.DueDate));

        // SentDate
        notEntity.Property(e => e.SentDate).HasColumnName(nameof(Notification.SentDate));
        repEntity.Property(e => e.SentDate).HasColumnName(nameof(Report.SentDate));

        // ReportsDeviations
        accEntity.Property(e => e.ReportsDeviations)
            .HasColumnName(nameof(AnnualComplianceCertification.ReportsDeviations));
        repEntity.Property(e => e.ReportsDeviations).HasColumnName(nameof(Report.ReportsDeviations));

        return builder;
    }

    internal static ModelBuilder ConfigureEnforcementActionMapping(this ModelBuilder builder)
    {
        // Enforcement Actions use "Table Per Hierarchy" (TPH) mapping strategy.
        builder.Entity<EnforcementAction>()
            .ToTable("EnforcementActions")
            .HasDiscriminator(action => action.ActionType)
            .HasValue<AdministrativeOrder>(EnforcementActionType.AdministrativeOrder)
            .HasValue<ConsentOrder>(EnforcementActionType.ConsentOrder)
            .HasValue<InformationalLetter>(EnforcementActionType.InformationalLetter)
            .HasValue<LetterOfNoncompliance>(EnforcementActionType.LetterOfNoncompliance)
            .HasValue<NoFurtherActionLetter>(EnforcementActionType.NoFurtherActionLetter)
            .HasValue<NoticeOfViolation>(EnforcementActionType.NoticeOfViolation)
            .HasValue<NovNfaLetter>(EnforcementActionType.NovNfaLetter)
            .HasValue<ProposedConsentOrder>(EnforcementActionType.ProposedConsentOrder);

        // Many-to-many relationships.
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-with-named-join-table
        builder.Entity<CaseFile>()
            .HasMany(caseFile => caseFile.ComplianceEvents)
            .WithMany(complianceEvent => complianceEvent.CaseFiles)
            .UsingEntity("CaseFileComplianceEvents");

        // TPH column sharing https://learn.microsoft.com/en-us/ef/core/modeling/inheritance#shared-columns
        var aorEntity = builder.Entity<AdministrativeOrder>();
        var corEntity = builder.Entity<ConsentOrder>();
        var infEntity = builder.Entity<InformationalLetter>();
        var lonEntity = builder.Entity<LetterOfNoncompliance>();
        var nnfEntity = builder.Entity<NovNfaLetter>();
        var novEntity = builder.Entity<NoticeOfViolation>();
        var pcoEntity = builder.Entity<ProposedConsentOrder>();

        // Executed date
        aorEntity.Property(e => e.ExecutedDate).HasColumnName(nameof(AdministrativeOrder.ExecutedDate));
        corEntity.Property(e => e.ExecutedDate).HasColumnName(nameof(ConsentOrder.ExecutedDate));

        // Resolved date
        aorEntity.Property(e => e.ResolvedDate).HasColumnName(nameof(AdministrativeOrder.ResolvedDate));
        corEntity.Property(e => e.ResolvedDate).HasColumnName(nameof(ConsentOrder.ResolvedDate));

        // Response requested
        infEntity.Property(e => e.ResponseRequested).HasColumnName(nameof(InformationalLetter.ResponseRequested));
        lonEntity.Property(e => e.ResponseRequested).HasColumnName(nameof(LetterOfNoncompliance.ResponseRequested));
        nnfEntity.Property(e => e.ResponseRequested).HasColumnName(nameof(NovNfaLetter.ResponseRequested));
        pcoEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(ProposedConsentOrder.ResponseReceived));

        // Response received
        infEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(InformationalLetter.ResponseReceived));
        lonEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(LetterOfNoncompliance.ResponseReceived));
        nnfEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(NovNfaLetter.ResponseReceived));
        novEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(NoticeOfViolation.ResponseReceived));
        pcoEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(ProposedConsentOrder.ResponseReceived));

        // Response received comment
        infEntity.Property(e => e.ResponseComment).HasColumnName(nameof(InformationalLetter.ResponseComment));
        lonEntity.Property(e => e.ResponseComment).HasColumnName(nameof(LetterOfNoncompliance.ResponseComment));
        nnfEntity.Property(e => e.ResponseComment).HasColumnName(nameof(NovNfaLetter.ResponseComment));
        novEntity.Property(e => e.ResponseComment).HasColumnName(nameof(NoticeOfViolation.ResponseComment));
        pcoEntity.Property(e => e.ResponseComment).HasColumnName(nameof(ProposedConsentOrder.ResponseComment));

        return builder;
    }

    internal static ModelBuilder ConfigureEnumValues(this ModelBuilder builder)
    {
        // == Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions

        // Work entries
        builder.Entity<WorkEntry>().Property(e => e.WorkEntryType).HasConversion<string>();
        builder.Entity<BaseInspection>().Property(e => e.InspectionReason).HasConversion<string>();
        builder.Entity<Report>().Property(e => e.ReportingPeriodType).HasConversion<string>();

        // Enforcement
        builder.Entity<CaseFile>().Property(e => e.CaseFileStatus).HasConversion<string>();
        builder.Entity<EnforcementAction>().Property(e => e.ActionType).HasConversion<string>();
        builder.Entity<EnforcementAction>().Property(e => e.Status).HasConversion<string>();
        builder.Entity<EnforcementActionReview>().Property(e => e.Result).HasConversion<string>();

        // Data exchange status
        builder.Entity<CaseFile>().Property(e => e.DataExchangeStatus).HasConversion<string>();
        builder.Entity<ComplianceEvent>().Property(e => e.DataExchangeStatus).HasConversion<string>();
        builder.Entity<ReportableEnforcementAction>().Property(e => e.DataExchangeStatus).HasConversion<string>();

        return builder;
    }

    internal static ModelBuilder ConfigureInheritanceMapping(this ModelBuilder builder)
    {
        // By default, EF maps inheritance using table-per-hierarchy (TPH), but we want to explicitly set the table names.
        builder.Entity<AuditPoint>().ToTable("AuditPoints");
        builder.Entity<Comment>().ToTable("Comments");
        builder.Entity<StandardNamedEntity>().ToTable("SelectLists");

        return builder;
    }

    internal static ModelBuilder ConfigureImpliedAddedChildEntities(this ModelBuilder builder)
    {
        // See https://github.com/dotnet/efcore/issues/35090#issuecomment-2485974295
        // and https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=fluent-api#no-value-generation
        builder.Entity<AuditPoint>().Property(e => e.Id).ValueGeneratedNever();
        builder.Entity<Comment>().Property(e => e.Id).ValueGeneratedNever();
        builder.Entity<EnforcementActionReview>().Property(e => e.Id).ValueGeneratedNever();
        builder.Entity<StipulatedPenalty>().Property(e => e.Id).ValueGeneratedNever();

        return builder;
    }

    internal static ModelBuilder ConfigureDateTimeOffsetHandling(this ModelBuilder builder, string? dbProviderName)
    {
        // == "Handling DateTimeOffset in SQLite with Entity Framework Core"
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        if (dbProviderName != AppDbContext.SqliteProvider) return builder;

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            // This doesn't work with owned types which we don't have anyway but which would be configured like so:
            // https://github.com/gaepdit/air-web/blob/ee621ee4708e7b4964e3aa66c34e04925cf80337/src/EfRepository/DbContext/AppDbContext.cs#L50-L68
            if (entityType.FindOwnership() != null) continue; // Skip owned types

            var dateTimeOffsetProperties = entityType.ClrType.GetProperties()
                .Where(info => info.PropertyType == typeof(DateTimeOffset) ||
                               info.PropertyType == typeof(DateTimeOffset?));
            foreach (var property in dateTimeOffsetProperties)
                builder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion(new DateTimeOffsetToBinaryConverter());
        }

        return builder;
    }

    internal static ModelBuilder ConfigureCollectionSerialization(this ModelBuilder builder)
    {
        // Ref: https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#collections-of-primitives

        // Case File pollutants: `List<string>`
        builder.Entity<CaseFile>()
            .Property(e => e.PollutantIds)
            .HasConversion(
                original => JsonSerializer.Serialize(original, JsonSerializerOptions.Default),
                serialized => JsonSerializer.Deserialize<List<string>>(serialized, JsonSerializerOptions.Default)!,
                new ValueComparer<ICollection<string>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        // Case File air programs: `List<AirProgram>`
        builder.Entity<CaseFile>()
            .Property(e => e.AirPrograms)
            .HasConversion(
                original => JsonSerializer.Serialize(original.Select(p => p.ToString()), JsonSerializerOptions.Default),
                serialized => JsonSerializer.Deserialize<List<string>>(serialized, JsonSerializerOptions.Default)!
                    .Select(Enum.Parse<AirProgram>).ToList(),
                new ValueComparer<ICollection<AirProgram>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        return builder;
    }

    internal static ModelBuilder ConfigureCompositeUniqueIndexes(this ModelBuilder builder)
    {
        // Configure composite unique indexes for FacilityId + ActionNumber
        // These ensure that ActionNumber is sequential and unique per FacilityId

        // ComplianceEvent (WorkEntry) - ActionNumber must be unique per FacilityId
        builder.Entity<ComplianceEvent>()
            .HasIndex(e => new { e.FacilityId, e.ActionNumber })
            .IsUnique();

        // Fce - ActionNumber must be unique per FacilityId
        builder.Entity<Fce>()
            .HasIndex(e => new { e.FacilityId, e.ActionNumber })
            .IsUnique();

        // CaseFile - ActionNumber must be unique per FacilityId
        builder.Entity<CaseFile>()
            .HasIndex(e => new { e.FacilityId, e.ActionNumber })
            .IsUnique();

        return builder;
    }
}
