using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AirWeb.EfRepository.DbContext.Configuration;

internal static class AppDbContextConfiguration
{
    internal static ModelBuilder ConfigureNavigationAutoIncludes(this ModelBuilder builder)
    {
        // Some properties should always be included.
        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations

        // Entities should include any maintenance properties.
        builder.Entity<ApplicationUser>().Navigation(user => user.Office).AutoInclude();
        builder.Entity<Notification>().Navigation(notification => notification.NotificationType).AutoInclude();

        // FCEs should include User data.
        var fceEntity = builder.Entity<Fce>();
        fceEntity.Navigation(fce => fce.DeletedBy).AutoInclude();
        fceEntity.Navigation(fce => fce.ReviewedBy).AutoInclude();

        // Work Entries should include User data.
        var workEntryEntity = builder.Entity<WorkEntry>();
        workEntryEntity.Navigation(entry => entry.ClosedBy).AutoInclude();
        workEntryEntity.Navigation(entry => entry.DeletedBy).AutoInclude();
        workEntryEntity.Navigation(entry => entry.ResponsibleStaff).AutoInclude();

        // Comments should include User data.
        builder.Entity<Comment>().Navigation(comment => comment.CommentBy).AutoInclude();

        return builder;
    }

    internal static ModelBuilder ConfigureTphDiscriminatorColumn(this ModelBuilder builder)
    {
        builder.Entity<WorkEntry>()
            .HasDiscriminator(entry => entry.WorkEntryType)
            .HasValue<WorkEntry>(WorkEntryType.Unknown)
            .HasValue<AnnualComplianceCertification>(WorkEntryType.AnnualComplianceCertification)
            .HasValue<Inspection>(WorkEntryType.Inspection)
            .HasValue<Notification>(WorkEntryType.Notification)
            .HasValue<PermitRevocation>(WorkEntryType.PermitRevocation)
            .HasValue<Report>(WorkEntryType.Report)
            .HasValue<RmpInspection>(WorkEntryType.RmpInspection)
            .HasValue<SourceTestReview>(WorkEntryType.SourceTestReview);
        return builder;
    }

    internal static ModelBuilder ConfigureTphColumnSharing(this ModelBuilder builder)
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
        insEntity.Property(e => e.ComplianceStatus).HasColumnName(nameof(Inspection.ComplianceStatus));
        rmpEntity.Property(e => e.ComplianceStatus).HasColumnName(nameof(RmpInspection.ComplianceStatus));

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

    internal static ModelBuilder ConfigureEnumValues(this ModelBuilder builder)
    {
        // == Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions
        builder.Entity<WorkEntry>().Property(entity => entity.WorkEntryType).HasConversion<string>();

        return builder;
    }

    internal static ModelBuilder ConfigureCalculatedColumns(this ModelBuilder builder, string? dbProviderName)
    {
        if (dbProviderName == AppDbContext.SqlServerProvider)
        {
            builder.Entity<WorkEntry>().Property(entry => entry.EventDate)
                .HasComputedColumnSql("""
                                      case
                                          when WorkEntryType = 'Unknown' then convert(date, CreatedAt)
                                          when WorkEntryType in ('AnnualComplianceCertification', 'Notification', 'PermitRevocation', 'Report')
                                              then convert(date, ReceivedDate)
                                          when WorkEntryType in ('Inspection', 'RmpInspection') then convert(date, InspectionStarted)
                                          when WorkEntryType = 'SourceTestReview' then convert(date, ReceivedByCompliance)
                                          else convert(date, '1900-1-1')
                                      end
                                      """);
        }
        else
        {
            builder.Entity<WorkEntry>().Property(entry => entry.EventDate)
                .HasComputedColumnSql("""
                                      case
                                          when WorkEntryType = 'Unknown' then date(CreatedAt)
                                          when WorkEntryType in ('AnnualComplianceCertification', 'Notification', 'PermitRevocation', 'Report')
                                              then date(ReceivedDate)
                                          when WorkEntryType in ('Inspection', 'RmpInspection') then date(InspectionStarted)
                                          when WorkEntryType = 'SourceTestReview' then date(ReceivedByCompliance)
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
