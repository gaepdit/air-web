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

        await MapWorkEntryDetailsAsync((BaseWorkEntryCreateDto)resource, workEntry, token).ConfigureAwait(false);
        return workEntry;
    }

    private async Task MapWorkEntryDetailsAsync(BaseWorkEntryCreateDto resource, BaseWorkEntry workEntry,
        CancellationToken token = default)
    {
        workEntry.Facility = await facilityRepository.GetFacilityAsync(resource.FacilityId, token)
            .ConfigureAwait(false);
        workEntry.ResponsibleStaff = resource.ResponsibleStaffId == null
            ? null
            : await userService.GetUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        workEntry.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        workEntry.Notes = resource.Notes;

        switch (workEntry.WorkEntryType)
        {
            case WorkEntryType.Notification:
                MapNotification((NotificationCreateDto)resource, (Notification)workEntry);
                break;
            case WorkEntryType.PermitRevocation:
                MapPermitRevocation((PermitRevocationCreateDto)resource, (PermitRevocation)workEntry);
                break;
            case WorkEntryType.ComplianceEvent:
                MapComplianceEventDetails(resource, (BaseComplianceEvent)workEntry);
                break;
            case WorkEntryType.Unknown:
            default:
                throw new ArgumentOutOfRangeException(nameof(workEntry), "Invalid work entry type.");
        }
    }

    private static void MapNotification(NotificationCreateDto resource, Notification workEntry)
    {
        workEntry.NotificationType = resource.NotificationType;
        workEntry.ReceivedDate = resource.ReceivedDate;
        workEntry.DueDate = resource.DueDate;
        workEntry.SentDate = resource.SentDate;
        workEntry.FollowupTaken = resource.FollowupTaken;
    }

    private static void MapPermitRevocation(PermitRevocationCreateDto resource, PermitRevocation workEntry)
    {
        workEntry.ReceivedDate = resource.ReceivedDate;
        workEntry.PermitRevocationDate = resource.PermitRevocationDate;
        workEntry.PhysicalShutdownDate = resource.PhysicalShutdownDate;
        workEntry.FollowupTaken = resource.FollowupTaken;
    }

    private static void MapComplianceEventDetails(BaseWorkEntryCreateDto resource, BaseComplianceEvent complianceEvent)
    {
        switch (complianceEvent.ComplianceEventType)
        {
            case ComplianceEventType.AnnualComplianceCertification:
                MapAcc((AccCreateDto)resource, (AnnualComplianceCertification)complianceEvent);
                break;
            case ComplianceEventType.Inspection:
                MapInspection((InspectionCreateDto)resource, (Inspection)complianceEvent);
                break;
            case ComplianceEventType.SourceTestReview:
                MapStr((SourceTestReviewCreateDto)resource, (SourceTestReview)complianceEvent);
                break;
            case ComplianceEventType.Report:
                MapReport((ReportCreateDto)resource, (Report)complianceEvent);
                break;
            case ComplianceEventType.RmpInspection:
                MapRmp((RmpInspectionCreateDto)resource, (RmpInspection)complianceEvent);
                break;
            case ComplianceEventType.Unknown:
            default:
                throw new ArgumentOutOfRangeException(nameof(complianceEvent), "Invalid compliance event type.");
        }
    }

    private static void MapAcc(AccCreateDto resource, AnnualComplianceCertification acc)
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

    private static void MapInspection(InspectionCreateDto resource, Inspection inspection)
    {
        inspection.InspectionReason = resource.InspectionReason;
        inspection.ComplianceStatus = resource.ComplianceStatus;
    }

    private static void MapReport(ReportCreateDto resource, Report report)
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

    private static void MapRmp(RmpInspectionCreateDto resource, RmpInspection inspection)
    {
        inspection.InspectionReason = resource.InspectionReason;
        inspection.ComplianceStatus = resource.ComplianceStatus;
    }

    private static void MapStr(SourceTestReviewCreateDto resource, SourceTestReview str)
    {
        str.ReferenceNumber = resource.ReferenceNumber;
        str.ReceivedByCompliance = resource.ReceivedByCompliance;
        str.DueDate = resource.DueDate;
        str.FollowupTaken = resource.FollowupTaken;
    }
}
