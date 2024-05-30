using AirWeb.AppServices.WorkEntries.Accs;
using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using AirWeb.AppServices.WorkEntries.Inspections;
using AirWeb.AppServices.WorkEntries.Notifications;
using AirWeb.AppServices.WorkEntries.PermitRevocations;
using AirWeb.AppServices.WorkEntries.Reports;
using AirWeb.AppServices.WorkEntries.RmpInspections;
using AirWeb.AppServices.WorkEntries.SourceTestReviews;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;

namespace AirWeb.AppServices.WorkEntries;

public sealed partial class WorkEntryService
{
    private async Task<BaseWorkEntry> CreateWorkEntryFromDtoAsync(IWorkEntryCreateDto resource,
        ApplicationUser? currentUser, CancellationToken token = default)
    {
        var workEntry = resource switch
        {
            AccCreateDto => workEntryManager.Create(WorkEntryType.ComplianceEvent, currentUser,
                ComplianceEventType.AnnualComplianceCertification),
            InspectionCreateDto => workEntryManager.Create(WorkEntryType.ComplianceEvent, currentUser,
                ComplianceEventType.Inspection),
            NotificationCreateDto => workEntryManager.Create(WorkEntryType.Notification, currentUser),
            PermitRevocationCreateDto => workEntryManager.Create(WorkEntryType.PermitRevocation, currentUser),
            ReportCreateDto => workEntryManager.Create(WorkEntryType.ComplianceEvent, currentUser,
                ComplianceEventType.Report),
            RmpInspectionCreateDto => workEntryManager.Create(WorkEntryType.ComplianceEvent, currentUser,
                ComplianceEventType.RmpInspection),
            SourceTestReviewCreateDto => workEntryManager.Create(WorkEntryType.ComplianceEvent, currentUser,
                ComplianceEventType.SourceTestReview),
            _ => throw new ArgumentException("Invalid create DTO resource."),
        };

        await MapWorkEntryDetailsAsync(resource, workEntry, token).ConfigureAwait(false);
        return workEntry;
    }

    private async Task MapWorkEntryDetailsAsync(IWorkEntryCreateDto resource, BaseWorkEntry workEntry,
        CancellationToken token = default)
    {
        workEntry.Facility = await facilityRepository.GetFacilityAsync(resource.FacilityId, token)
            .ConfigureAwait(false);
        workEntry.ResponsibleStaff = resource.ResponsibleStaffId == null
            ? null
            : await userService.GetUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        workEntry.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        workEntry.Notes = resource.Notes;

        switch (resource)
        {
            case NotificationCreateDto dto:
                MapNotification(dto, (Notification)workEntry);
                break;
            case PermitRevocationCreateDto dto:
                MapPermitRevocation(dto, (PermitRevocation)workEntry);
                break;
            case AccCreateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)workEntry);
                break;
            case InspectionCreateDto dto:
                MapInspection(dto, (Inspection)workEntry);
                break;
            case SourceTestReviewCreateDto dto:
                MapStr(dto, (SourceTestReview)workEntry);
                break;
            case ReportCreateDto dto:
                MapReport(dto, (Report)workEntry);
                break;
            case RmpInspectionCreateDto dto:
                MapRmp(dto, (RmpInspection)workEntry);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(workEntry), "Invalid work entry type.");
        }
    }

    private async Task UpdateWorkEntryFromDtoAsync(IWorkEntryUpdateDto resource, BaseWorkEntry workEntry)
    {
        workEntry.ResponsibleStaff = resource.ResponsibleStaffId == null
            ? null
            : await userService.GetUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        workEntry.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        workEntry.Notes = resource.Notes;

        switch (resource)
        {
            case NotificationUpdateDto dto:
                MapNotification(dto, (Notification)workEntry);
                break;
            case PermitRevocationUpdateDto dto:
                MapPermitRevocation(dto, (PermitRevocation)workEntry);
                break;
            case AccUpdateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)workEntry);
                break;
            case InspectionUpdateDto dto:
                MapInspection(dto, (Inspection)workEntry);
                break;
            case SourceTestReviewUpdateDto dto:
                MapStr(dto, (SourceTestReview)workEntry);
                break;
            case ReportUpdateDto dto:
                MapReport(dto, (Report)workEntry);
                break;
            case RmpInspectionUpdateDto dto:
                MapRmp(dto, (RmpInspection)workEntry);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(workEntry), "Invalid work entry type.");
        }
    }

    private static void MapNotification(INotificationCommandDto resource, Notification workEntry)
    {
        workEntry.NotificationType = resource.NotificationType;
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

    private static void MapAcc(IAccCommandDto resource, AnnualComplianceCertification acc)
    {
        acc.ReceivedDate = resource.ReceivedDate;
        acc.AccReportingYear = resource.AccReportingYear;
        acc.Postmarked = resource.Postmarked;
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

    private static void MapInspection(IInspectionCommandDto resource, Inspection inspection)
    {
        inspection.InspectionReason = resource.InspectionReason;
        inspection.ComplianceStatus = resource.ComplianceStatus;
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

    private static void MapRmp(IRmpInspectionCommandDto resource, RmpInspection inspection)
    {
        inspection.InspectionReason = resource.InspectionReason;
        inspection.ComplianceStatus = resource.ComplianceStatus;
    }

    private static void MapStr(ISourceTestReviewCommandDto resource, SourceTestReview str)
    {
        str.ReferenceNumber = resource.ReferenceNumber;
        str.ReceivedByCompliance = resource.ReceivedByCompliance;
        str.DueDate = resource.DueDate;
        str.FollowupTaken = resource.FollowupTaken;
    }
}