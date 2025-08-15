using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Identity;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public sealed partial class WorkEntryService
{
    private async Task<WorkEntry> CreateWorkEntryFromDtoAsync(IWorkEntryCreateDto resource,
        ApplicationUser? currentUser, CancellationToken token = default)
    {
        var facilityId = (FacilityId)resource.FacilityId!;
        var workEntry = resource switch
        {
            AccCreateDto => entryManager.Create(WorkEntryType.AnnualComplianceCertification, facilityId, currentUser),
            InspectionCreateDto dto => dto.IsRmpInspection
                ? entryManager.Create(WorkEntryType.RmpInspection, facilityId, currentUser)
                : entryManager.Create(WorkEntryType.Inspection, facilityId, currentUser),
            NotificationCreateDto => entryManager.Create(WorkEntryType.Notification, facilityId, currentUser),
            PermitRevocationCreateDto => entryManager.Create(WorkEntryType.PermitRevocation, facilityId, currentUser),
            ReportCreateDto => entryManager.Create(WorkEntryType.Report, facilityId, currentUser),
            SourceTestReviewCreateDto => entryManager.Create(WorkEntryType.SourceTestReview, facilityId, currentUser),
            _ => throw new ArgumentException("Invalid create DTO resource."),
        };

        workEntry.ResponsibleStaff = await userService.GetUserAsync(resource.ResponsibleStaffId!).ConfigureAwait(false);
        workEntry.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        workEntry.Notes = resource.Notes ?? string.Empty;

        switch (resource)
        {
            case AccCreateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)workEntry);
                break;
            case InspectionCreateDto dto:
                MapInspection(dto, (BaseInspection)workEntry);
                break;
            case NotificationCreateDto dto:
                await MapNotificationAsync(dto, (Notification)workEntry, token).ConfigureAwait(false);
                break;
            case PermitRevocationCreateDto dto:
                MapPermitRevocation(dto, (PermitRevocation)workEntry);
                break;
            case ReportCreateDto dto:
                MapReport(dto, (Report)workEntry);
                break;
            case SourceTestReviewCreateDto dto:
                ((SourceTestReview)workEntry).ReferenceNumber = dto.ReferenceNumber;
                MapStr(dto, (SourceTestReview)workEntry);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), "Invalid work entry type.");
        }

        return workEntry;
    }

    private async Task UpdateWorkEntryFromDtoAsync(IWorkEntryCommandDto resource, WorkEntry workEntry,
        CancellationToken token)
    {
        workEntry.ResponsibleStaff = resource.ResponsibleStaffId == null
            ? null
            : await userService.GetUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        workEntry.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        workEntry.Notes = resource.Notes ?? string.Empty;

        switch (resource)
        {
            case AccUpdateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)workEntry);
                break;

            case InspectionUpdateDto dto:
                MapInspection(dto, (BaseInspection)workEntry);
                break;

            case NotificationUpdateDto dto:
                await MapNotificationAsync(dto, (Notification)workEntry, token).ConfigureAwait(false);
                break;

            case PermitRevocationUpdateDto dto:
                MapPermitRevocation(dto, (PermitRevocation)workEntry);
                break;

            case ReportUpdateDto dto:
                MapReport(dto, (Report)workEntry);
                break;

            case SourceTestReviewUpdateDto dto:
                MapStr(dto, (SourceTestReview)workEntry);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(workEntry), "Invalid work entry type.");
        }
    }

    private static void MapAcc(IAccCommandDto resource, AnnualComplianceCertification acc)
    {
        acc.ReceivedDate = resource.ReceivedDate;
        acc.AccReportingYear = resource.AccReportingYear;
        acc.PostmarkDate = resource.PostmarkDate;
        acc.PostmarkedOnTime = resource.PostmarkedOnTime;
        acc.SignedByRo = resource.SignedByRo;
        acc.OnCorrectForms = resource.OnCorrectForms;
        acc.IncludesAllTvConditions = resource.IncludesAllTvConditions;
        acc.CorrectlyCompleted = resource.CorrectlyCompleted;
        acc.ReportsDeviations = resource.ReportsDeviations;
        acc.IncludesPreviouslyUnreportedDeviations = resource.IncludesPreviouslyUnreportedDeviations;
        acc.ReportsAllKnownDeviations = resource.ReportsAllKnownDeviations;
        acc.ResubmittalRequired = resource.ResubmittalRequired;
        acc.EnforcementNeeded = resource.EnforcementNeeded;
    }

    private static void MapInspection(IInspectionCommandDto resource, BaseInspection inspection)
    {
        inspection.InspectionReason = resource.InspectionReason;
        inspection.InspectionStarted = resource.InspectionStartedDate.ToDateTime(resource.InspectionStartedTime);
        inspection.InspectionEnded = resource.InspectionEndedDate.ToDateTime(resource.InspectionEndedTime);
        inspection.WeatherConditions = resource.WeatherConditions ?? string.Empty;
        inspection.InspectionGuide = resource.InspectionGuide ?? string.Empty;
        inspection.FacilityOperating = resource.FacilityOperating;
        inspection.DeviationsNoted = resource.DeviationsNoted;
        inspection.FollowupTaken = resource.FollowupTaken;
    }

    private async Task MapNotificationAsync(INotificationCommandDto resource, Notification workEntry,
        CancellationToken token)
    {
        workEntry.NotificationType = await entryRepository
            .GetNotificationTypeAsync(resource.NotificationTypeId!.Value, token).ConfigureAwait(false);
        workEntry.ReceivedDate = resource.ReceivedDate;
        workEntry.DueDate = resource.DueDate;
        workEntry.SentDate = resource.SentDate;
        workEntry.FollowupTaken = resource.FollowupTaken;
    }

    private static void MapPermitRevocation(IPermitRevocationCommandDto resource, PermitRevocation workEntry)
    {
        workEntry.ReceivedDate = resource.ReceivedDate;
        workEntry.PermitRevocationDate = resource.PermitRevocationDate;
        workEntry.PhysicalShutdownDate = resource.PhysicalShutdownDate;
        workEntry.FollowupTaken = resource.FollowupTaken;
    }

    private static void MapReport(IReportCommandDto resource, Report report)
    {
        report.ReceivedDate = resource.ReceivedDate;
        report.ReportingPeriodType = resource.ReportingPeriodType;
        report.ReportingPeriodStart = resource.ReportingPeriodStart;
        report.ReportingPeriodEnd = resource.ReportingPeriodEnd;
        report.ReportingPeriodComment = resource.ReportingPeriodComment;
        report.DueDate = resource.DueDate;
        report.SentDate = resource.SentDate;
        report.ReportComplete = resource.ReportComplete;
        report.ReportsDeviations = resource.ReportsDeviations;
        report.EnforcementNeeded = resource.EnforcementNeeded;
    }

    private static void MapStr(ISourceTestReviewCommandDto resource, SourceTestReview str)
    {
        str.ReceivedByComplianceDate = resource.ReceivedByComplianceDate!.Value;
        str.DueDate = resource.DueDate;
        str.FollowupTaken = resource.FollowupTaken;
    }
}
