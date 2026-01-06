using AirWeb.AppServices.Compliance.ComplianceMonitoring.Accs;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Inspections;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Reports;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Identity;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring;

public sealed partial class ComplianceWorkService
{
    private async Task<ComplianceWork> CreateComplianceWorkFromDtoAsync(IComplianceWorkCreateDto resource,
        ApplicationUser? currentUser, CancellationToken token = default)
    {
        var facilityId = (FacilityId)resource.FacilityId!;
        var workTask = resource switch
        {
            AccCreateDto => manager.CreateAsync(ComplianceWorkType.AnnualComplianceCertification, facilityId,
                currentUser),
            InspectionCreateDto dto => dto.IsRmpInspection
                ? manager.CreateAsync(ComplianceWorkType.RmpInspection, facilityId, currentUser)
                : manager.CreateAsync(ComplianceWorkType.Inspection, facilityId, currentUser),
            NotificationCreateDto => manager.CreateAsync(ComplianceWorkType.Notification, facilityId, currentUser),
            PermitRevocationCreateDto => manager.CreateAsync(ComplianceWorkType.PermitRevocation, facilityId,
                currentUser),
            ReportCreateDto => manager.CreateAsync(ComplianceWorkType.Report, facilityId, currentUser),
            SourceTestReviewCreateDto => manager.CreateAsync(ComplianceWorkType.SourceTestReview, facilityId,
                currentUser),
            _ => throw new ArgumentException("Invalid create DTO resource."),
        };

        var work = await workTask.ConfigureAwait(false);
        work.ResponsibleStaff = await userService.GetUserAsync(resource.ResponsibleStaffId!).ConfigureAwait(false);
        work.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        work.Notes = resource.Notes ?? string.Empty;

        switch (resource)
        {
            case AccCreateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)work);
                break;
            case InspectionCreateDto dto:
                MapInspection(dto, (BaseInspection)work);
                break;
            case NotificationCreateDto dto:
                await MapNotificationAsync(dto, (Notification)work, token).ConfigureAwait(false);
                break;
            case PermitRevocationCreateDto dto:
                MapPermitRevocation(dto, (PermitRevocation)work);
                break;
            case ReportCreateDto dto:
                MapReport(dto, (Report)work);
                break;
            case SourceTestReviewCreateDto dto:
                ((SourceTestReview)work).ReferenceNumber = dto.ReferenceNumber;
                MapStr(dto, (SourceTestReview)work);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), "Invalid compliance work type.");
        }

        return work;
    }

    private async Task UpdateComplianceWorkFromDtoAsync(IComplianceWorkCommandDto resource, ComplianceWork work,
        CancellationToken token)
    {
        work.ResponsibleStaff = resource.ResponsibleStaffId == null
            ? null
            : await userService.GetUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        work.AcknowledgmentLetterDate = resource.AcknowledgmentLetterDate;
        work.Notes = resource.Notes ?? string.Empty;

        switch (resource)
        {
            case AccUpdateDto dto:
                MapAcc(dto, (AnnualComplianceCertification)work);
                break;

            case InspectionUpdateDto dto:
                MapInspection(dto, (BaseInspection)work);
                break;

            case NotificationUpdateDto dto:
                await MapNotificationAsync(dto, (Notification)work, token).ConfigureAwait(false);
                break;

            case PermitRevocationUpdateDto dto:
                MapPermitRevocation(dto, (PermitRevocation)work);
                break;

            case ReportUpdateDto dto:
                MapReport(dto, (Report)work);
                break;

            case SourceTestReviewUpdateDto dto:
                MapStr(dto, (SourceTestReview)work);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(work), "Invalid compliance work type.");
        }
    }

    private static void MapAcc(IAccCommandDto resource, AnnualComplianceCertification work)
    {
        work.ReceivedDate = resource.ReceivedDate;
        work.AccReportingYear = resource.AccReportingYear;
        work.PostmarkDate = resource.PostmarkDate;
        work.PostmarkedOnTime = resource.PostmarkedOnTime;
        work.SignedByRo = resource.SignedByRo;
        work.OnCorrectForms = resource.OnCorrectForms;
        work.IncludesAllTvConditions = resource.IncludesAllTvConditions;
        work.CorrectlyCompleted = resource.CorrectlyCompleted;
        work.ReportsDeviations = resource.ReportsDeviations;
        work.IncludesPreviouslyUnreportedDeviations = resource.IncludesPreviouslyUnreportedDeviations;
        work.ReportsAllKnownDeviations = resource.ReportsAllKnownDeviations;
        work.ResubmittalRequired = resource.ResubmittalRequired;
        work.EnforcementNeeded = resource.EnforcementNeeded;
    }

    private static void MapInspection(IInspectionCommandDto resource, BaseInspection work)
    {
        work.InspectionReason = resource.InspectionReason;
        work.InspectionStarted = resource.InspectionStartedDate.ToDateTime(resource.InspectionStartedTime);
        work.InspectionEnded = resource.InspectionEndedDate.ToDateTime(resource.InspectionEndedTime);
        work.WeatherConditions = resource.WeatherConditions ?? string.Empty;
        work.InspectionGuide = resource.InspectionGuide ?? string.Empty;
        work.FacilityOperating = resource.FacilityOperating;
        work.DeviationsNoted = resource.DeviationsNoted;
        work.FollowupTaken = resource.FollowupTaken;
    }

    private async Task MapNotificationAsync(INotificationCommandDto resource, Notification work,
        CancellationToken token)
    {
        work.NotificationType = await repository
            .GetNotificationTypeAsync(resource.NotificationTypeId!.Value, token).ConfigureAwait(false);
        work.ReceivedDate = resource.ReceivedDate;
        work.DueDate = resource.DueDate;
        work.SentDate = resource.SentDate;
        work.FollowupTaken = resource.FollowupTaken;
    }

    private static void MapPermitRevocation(IPermitRevocationCommandDto resource, PermitRevocation work)
    {
        work.ReceivedDate = resource.ReceivedDate;
        work.PermitRevocationDate = resource.PermitRevocationDate;
        work.PhysicalShutdownDate = resource.PhysicalShutdownDate;
        work.FollowupTaken = resource.FollowupTaken;
    }

    private static void MapReport(IReportCommandDto resource, Report work)
    {
        work.ReceivedDate = resource.ReceivedDate;
        work.ReportingPeriodType = resource.ReportingPeriodType;
        work.ReportingPeriodStart = resource.ReportingPeriodStart;
        work.ReportingPeriodEnd = resource.ReportingPeriodEnd;
        work.ReportingPeriodComment = resource.ReportingPeriodComment;
        work.DueDate = resource.DueDate;
        work.SentDate = resource.SentDate;
        work.ReportComplete = resource.ReportComplete;
        work.ReportsDeviations = resource.ReportsDeviations;
        work.EnforcementNeeded = resource.EnforcementNeeded;
    }

    private static void MapStr(ISourceTestReviewCommandDto resource, SourceTestReview work)
    {
        work.ReceivedByComplianceDate = resource.ReceivedByComplianceDate!.Value;
        work.DueDate = resource.DueDate;
        work.FollowupTaken = resource.FollowupTaken;
    }
}
