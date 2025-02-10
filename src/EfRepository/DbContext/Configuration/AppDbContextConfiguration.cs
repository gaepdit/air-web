using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AirWeb.EfRepository.DbContext.Configuration;

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
        enforcementActionEntity.Navigation(enforcementAction => enforcementAction.CurrentReviewer).AutoInclude();

        var enforcementActionReviewEntity = builder.Entity<EnforcementActionReview>();
        enforcementActionReviewEntity.Navigation(review => review.ReviewedBy).AutoInclude();

        // Comments
        builder.Entity<Comment>().Navigation(comment => comment.CommentBy).AutoInclude();

        return builder;
    }

    internal static ModelBuilder ConfigureWorkEntryMapping(this ModelBuilder builder)
    {
        // Work Entries use Table Per Hierarchy (TPH) mapping strategy.
        builder.Entity<WorkEntry>()
            .UseTphMappingStrategy() // This is already the default, but making it explicit here for future clarity.
            .ToTable("WorkEntries")
            .HasDiscriminator(entry => entry.WorkEntryType)
            .HasValue<AnnualComplianceCertification>(WorkEntryType.AnnualComplianceCertification)
            .HasValue<Inspection>(WorkEntryType.Inspection)
            .HasValue<Notification>(WorkEntryType.Notification)
            .HasValue<PermitRevocation>(WorkEntryType.PermitRevocation)
            .HasValue<Report>(WorkEntryType.Report)
            .HasValue<RmpInspection>(WorkEntryType.RmpInspection)
            .HasValue<SourceTestReview>(WorkEntryType.SourceTestReview);

        return builder.ConfigureWorkEntryColumnSharing();
    }

    private static ModelBuilder ConfigureWorkEntryColumnSharing(this ModelBuilder builder)
    {
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
        // Enforcement Actions use Table Per Hierarchy (TPH) mapping strategy.
        builder.Entity<EnforcementAction>()
            .UseTphMappingStrategy() // This is already the default, but making it explicit here for future clarity.
            .ToTable("EnforcementActions")
            .HasDiscriminator(action => action.ActionType)
            .HasValue<AdministrativeOrder>(EnforcementActionType.AdministrativeOrder)
            .HasValue<AoResolvedLetter>(EnforcementActionType.AoResolvedLetter)
            .HasValue<ConsentOrder>(EnforcementActionType.ConsentOrder)
            .HasValue<CoResolvedLetter>(EnforcementActionType.CoResolvedLetter)
            .HasValue<InformationalLetter>(EnforcementActionType.InformationalLetter)
            .HasValue<LetterOfNoncompliance>(EnforcementActionType.LetterOfNoncompliance)
            .HasValue<NoFurtherActionLetter>(EnforcementActionType.NoFurtherAction)
            .HasValue<NoticeOfViolation>(EnforcementActionType.NoticeOfViolation)
            .HasValue<NovNfaLetter>(EnforcementActionType.NovNfaLetter)
            .HasValue<ProposedConsentOrder>(EnforcementActionType.ProposedConsentOrder);

        // Many-to-many relationships.
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-with-named-join-table
        builder.Entity<CaseFile>()
            .HasMany(caseFile => caseFile.ComplianceEvents)
            .WithMany(complianceEvent => complianceEvent.CaseFiles)
            .UsingEntity("CaseFileComplianceEvents");

        // Self-referencing relationships.
        builder.Entity<AdministrativeOrder>()
            .HasOne(order => order.ResolvedLetter)
            .WithOne(letter => letter.AdministrativeOrder)
            .HasForeignKey<AoResolvedLetter>("AdministrativeOrderId")
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<ConsentOrder>()
            .HasOne(order => order.ResolvedLetter)
            .WithOne(letter => letter.ConsentOrder)
            .HasForeignKey<CoResolvedLetter>("ConsentOrderId")
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<NoticeOfViolation>()
            .HasOne(noticeOfViolation => noticeOfViolation.NoFurtherActionLetter)
            .WithOne(letter => letter.NoticeOfViolation)
            .HasForeignKey<NoFurtherActionLetter>("NoticeOfViolationId_for_Nfa")
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<ProposedConsentOrder>()
            .HasOne(letter => letter.NoticeOfViolation)
            .WithMany()
            .HasForeignKey("NoticeOfViolationId_for_ProposedCo")
            .OnDelete(DeleteBehavior.ClientCascade);

        return builder.ConfigureEnforcementActionColumnSharing();
    }

    private static ModelBuilder ConfigureEnforcementActionColumnSharing(this ModelBuilder builder)
    {
        // TPH column sharing https://learn.microsoft.com/en-us/ef/core/modeling/inheritance#shared-columns

        var aorEntity = builder.Entity<AdministrativeOrder>();
        var corEntity = builder.Entity<ConsentOrder>();
        var nelEntity = builder.Entity<InformationalLetter>();
        var lonEntity = builder.Entity<LetterOfNoncompliance>();
        var nnlEntity = builder.Entity<NovNfaLetter>();
        var novEntity = builder.Entity<NoticeOfViolation>();
        var pcoEntity = builder.Entity<ProposedConsentOrder>();

        // Executed date
        aorEntity.Property(e => e.ExecutedDate).HasColumnName(nameof(AdministrativeOrder.ExecutedDate));
        corEntity.Property(e => e.ExecutedDate).HasColumnName(nameof(ConsentOrder.ExecutedDate));

        // Resolved date
        aorEntity.Property(e => e.ResolvedDate).HasColumnName(nameof(AdministrativeOrder.ResolvedDate));
        corEntity.Property(e => e.ResolvedDate).HasColumnName(nameof(ConsentOrder.ResolvedDate));

        // Response requested
        nelEntity.Property(e => e.ResponseRequested).HasColumnName(nameof(InformationalLetter.ResponseRequested));
        lonEntity.Property(e => e.ResponseRequested).HasColumnName(nameof(LetterOfNoncompliance.ResponseRequested));
        nnlEntity.Property(e => e.ResponseRequested).HasColumnName(nameof(NovNfaLetter.ResponseRequested));
        novEntity.Property(e => e.ResponseRequested).HasColumnName(nameof(NovNfaLetter.ResponseRequested));

        // Response received
        nelEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(InformationalLetter.ResponseReceived));
        lonEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(LetterOfNoncompliance.ResponseReceived));
        nnlEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(NovNfaLetter.ResponseReceived));
        novEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(NovNfaLetter.ResponseReceived));
        pcoEntity.Property(e => e.ResponseReceived).HasColumnName(nameof(ProposedConsentOrder.ResponseReceived));

        return builder;
    }

    internal static ModelBuilder ConfigureEnumValues(this ModelBuilder builder)
    {
        // == Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions

        // Discriminator
        builder.Entity<WorkEntry>().Property(e => e.WorkEntryType).HasConversion<string>();
        builder.Entity<EnforcementAction>().Property(e => e.ActionType).HasConversion<string>();

        // Status
        builder.Entity<CaseFile>().Property(e => e.CaseFileStatus).HasConversion<string>();
        builder.Entity<EnforcementActionReview>().Property(e => e.Result).HasConversion<string>();

        // Data exchange status
        builder.Entity<CaseFile>().Property(e => e.DataExchangeStatus).HasConversion<string>();
        builder.Entity<ComplianceEvent>().Property(e => e.DataExchangeStatus).HasConversion<string>();
        builder.Entity<EnforcementAction>().Property(e => e.DataExchangeStatus).HasConversion<string>();

        return builder;
    }

    internal static ModelBuilder ConfigureCalculatedColumns(this ModelBuilder builder, string? dbProviderName)
    {
        if (dbProviderName == AppDbContext.SqlServerProvider)
        {
            builder.Entity<WorkEntry>().Property(entry => entry.EventDate)
                .HasComputedColumnSql("""
                                      case
                                          when WorkEntryType in ('AnnualComplianceCertification', 'Notification', 'PermitRevocation', 'Report')
                                              then convert(date, ReceivedDate)
                                          when WorkEntryType in ('Inspection', 'RmpInspection') then convert(date, InspectionStarted)
                                          when WorkEntryType = 'SourceTestReview' then convert(date, ReceivedByComplianceDate)
                                          else convert(date, '1900-1-1')
                                      end
                                      """);
        }
        else
        {
            builder.Entity<WorkEntry>().Property(entry => entry.EventDate)
                .HasComputedColumnSql("""
                                      case
                                          when WorkEntryType in ('AnnualComplianceCertification', 'Notification', 'PermitRevocation', 'Report')
                                              then date(ReceivedDate)
                                          when WorkEntryType in ('Inspection', 'RmpInspection') then date(InspectionStarted)
                                          when WorkEntryType = 'SourceTestReview' then date(ReceivedByComplianceDate)
                                          else '1900-1-1'
                                      end
                                      """);
        }

        return builder;
    }

    internal static ModelBuilder ConfigureCommentsMappingStrategy(this ModelBuilder builder)
    {
        // Use TPH strategy for Comments table (this doesn't happen automatically because the Comment class is not 
        // directly exposed as a DbSet).
        builder.Entity<Comment>().UseTphMappingStrategy().ToTable("Comments");

        return builder;
    }

    internal static ModelBuilder ConfigureDateTimeOffsetHandling(this ModelBuilder builder, string? dbProviderName)
    {
        // == "Handling DateTimeOffset in SQLite with Entity Framework Core"
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        if (dbProviderName != AppDbContext.SqliteProvider) return builder;

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

        return builder;
    }
}
