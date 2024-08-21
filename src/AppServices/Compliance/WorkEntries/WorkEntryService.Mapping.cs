using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public sealed partial class WorkEntryService
{
    private async Task<WorkEntry> CreateWorkEntryFromDtoAsync(IWorkEntryCreateDto resource,
        ApplicationUser? currentUser, CancellationToken token = default)
    {
        var workEntry = resource switch
        {
            AccCreateDto => workEntryManager.Create(WorkEntryType.AnnualComplianceCertification, currentUser),
            InspectionCreateDto => workEntryManager.Create(WorkEntryType.Inspection, currentUser),
            NotificationCreateDto => workEntryManager.Create(WorkEntryType.Notification, currentUser),
            PermitRevocationCreateDto => workEntryManager.Create(WorkEntryType.PermitRevocation, currentUser),
            ReportCreateDto => workEntryManager.Create(WorkEntryType.Report, currentUser),
            RmpInspectionCreateDto => workEntryManager.Create(WorkEntryType.RmpInspection, currentUser),
            SourceTestReviewCreateDto => workEntryManager.Create(WorkEntryType.SourceTestReview, currentUser),
            _ => throw new ArgumentException("Invalid create DTO resource."),
        };

        await MapWorkEntryDetailsAsync(resource, workEntry, token).ConfigureAwait(false);
        return workEntry;
    }

    private async Task MapWorkEntryDetailsAsync(IWorkEntryCreateDto resource, WorkEntry workEntry,
        CancellationToken token = default)
    {
        workEntry.Facility = await facilityRepository.GetFacilityAsync((FacilityId)resource.FacilityId!, token)
            .ConfigureAwait(false);
        workEntry.ResponsibleStaff = resource.ResponsibleStaffId == null
            ? null
            : await userService.GetUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        workEntry.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        workEntry.Notes = resource.Notes;

        switch (resource)
        {
            case AccCreateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)workEntry);
                break;
            case InspectionCreateDto dto:
                MapInspection(dto, (Inspection)workEntry);
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
            case RmpInspectionCreateDto dto:
                MapRmp(dto, (RmpInspection)workEntry);
                break;
            case SourceTestReviewCreateDto dto:
                MapStr(dto, (SourceTestReview)workEntry);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(workEntry), "Invalid work entry type.");
        }
    }

    private async Task UpdateWorkEntryFromDtoAsync(IWorkEntryUpdateDto resource, WorkEntry workEntry,
        CancellationToken token)
    {
        workEntry.ResponsibleStaff = resource.ResponsibleStaffId == null
            ? null
            : await userService.GetUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        workEntry.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        workEntry.Notes = resource.Notes;

        switch (resource)
        {
            case AccUpdateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)workEntry);
                break;

            case InspectionUpdateDto dto:
                if (workEntry.WorkEntryType == WorkEntryType.Inspection)
                    MapInspection(dto, (Inspection)workEntry);
                else
                    MapRmp(dto, (RmpInspection)workEntry);
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
        inspection.DeviationsNoted = resource.DeviationsNoted;
    }

    private async Task MapNotificationAsync(INotificationCommandDto resource, Notification workEntry,
        CancellationToken token)
    {
        workEntry.NotificationType = await workEntryRepository
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

    private static void MapRmp(IInspectionCommandDto resource, RmpInspection inspection)
    {
        inspection.InspectionReason = resource.InspectionReason;
        inspection.DeviationsNoted = resource.DeviationsNoted;
    }

    private static void MapStr(ISourceTestReviewCommandDto resource, SourceTestReview str)
    {
        str.ReferenceNumber = resource.ReferenceNumber;
        str.ReceivedByCompliance = resource.ReceivedByCompliance;
        str.DueDate = resource.DueDate;
        str.FollowupTaken = resource.FollowupTaken;
    }
}
