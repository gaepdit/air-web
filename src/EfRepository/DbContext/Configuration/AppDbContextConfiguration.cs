using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;
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

        // FCEs should include all User data.
        var fceEntity = builder.Entity<Fce>();
        fceEntity.Navigation(fce => fce.DeletedBy).AutoInclude();
        fceEntity.Navigation(fce => fce.ReviewedBy).AutoInclude();

        // Work Entries should include all User data.
        var workEntryEntity = builder.Entity<WorkEntry>();
        workEntryEntity.Navigation(entry => entry.ClosedBy).AutoInclude();
        workEntryEntity.Navigation(entry => entry.DeletedBy).AutoInclude();
        workEntryEntity.Navigation(entry => entry.ResponsibleStaff).AutoInclude();

        return builder;
    }

    internal static ModelBuilder ConfigureEnumValues(this ModelBuilder builder)
    {
        // == Let's save enums in the database as strings.
        // See https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#pre-defined-conversions
        builder.Entity<WorkEntry>().Property(entity => entity.WorkEntryType).HasConversion<string>();
        builder.Entity<ComplianceEvent>().Property(entity => entity.ComplianceEventType).HasConversion<string>();

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

    internal static ModelBuilder ConfigureOwnedTypeCollections(this ModelBuilder builder, string? dbProviderName)
    {
        // == Collections of owned types
        // TODO: Try using a hierarchy of inherited Comment types and see if TPH can be used to put all Comments in a single table.
        // Collections of the same owned types by different owners should be stored in different tables.
        // Comments should include all User data.
        // Sqlite and EF Core are in conflict on how to handle collections of owned types.
        // See: https://stackoverflow.com/a/69826156/212978
        // and: https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
        builder.Entity<Fce>().OwnsMany(fce => fce.Comments, owned =>
        {
            owned.ToTable("Fce_Comments");
            owned.Navigation(comment => comment.CommentBy).AutoInclude();
            if (dbProviderName != AppDbContext.SqliteProvider) return;
            owned.HasKey(propertyNames: "Id");
            owned.Property(comment => comment.CommentedAt).HasConversion(new DateTimeOffsetToBinaryConverter());
        });

        builder.Entity<WorkEntry>().OwnsMany(entry => entry.Comments, owned =>
        {
            owned.ToTable("WorkEntry_Comments");
            owned.Navigation(comment => comment.CommentBy).AutoInclude();
            if (dbProviderName != AppDbContext.SqliteProvider) return;
            owned.HasKey(propertyNames: "Id");
            owned.Property(comment => comment.CommentedAt).HasConversion(new DateTimeOffsetToBinaryConverter());
        });

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
