using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.EfRepository.DbContext.Configuration;

internal static class AppDbContextConfiguration
{
    internal static void ConfigureTphColumnSharing(ModelBuilder builder)
    {
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
    }
}
