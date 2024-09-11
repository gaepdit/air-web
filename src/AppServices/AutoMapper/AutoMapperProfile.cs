using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.Domain.ValueObjects;
using AutoMapper;

namespace AirWeb.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        Users();
        MaintenanceItems();
        Comments();
        Facilities();
        Fces();
        Accs();
        Inspections();
        Notifications();
        PermitRevocations();
        Reports();
        RmpInspections();
        SourceTestReviews();
        SearchResults();
    }

    private void Users()
    {
        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();
    }

    private void MaintenanceItems()
    {
        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();

        CreateMap<NotificationType, NotificationTypeUpdateDto>();
        CreateMap<NotificationType, NotificationTypeViewDto>();
    }

    private void Comments()
    {
        CreateMap<Comment, CommentViewDto>();
    }

    private void Facilities()
    {
        CreateMap<Facility, FacilityViewDto>();
    }

    private void Fces()
    {
        CreateMap<Fce, FceUpdateDto>();
        CreateMap<Fce, FceSummaryDto>();
        CreateMap<Fce, FceViewDto>();
    }

    private void Accs()
    {
        CreateMap<AnnualComplianceCertification, AccUpdateDto>();
        CreateMap<AnnualComplianceCertification, AccViewDto>();
    }

    private void Inspections()
    {
        CreateMap<Inspection, InspectionUpdateDto>()
            .ForMember(dto => dto.InspectionStartedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionStarted.Date)))
            .ForMember(dto => dto.InspectionStartedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionStarted.TimeOfDay)))
            .ForMember(dto => dto.InspectionEndedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionEnded.Date)))
            .ForMember(dto => dto.InspectionEndedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionEnded.TimeOfDay)));
        CreateMap<Inspection, InspectionViewDto>();
    }

    private void Notifications()
    {
        CreateMap<Notification, NotificationUpdateDto>();
        CreateMap<Notification, NotificationViewDto>();
    }

    private void PermitRevocations()
    {
        CreateMap<PermitRevocation, PermitRevocationUpdateDto>();
        CreateMap<PermitRevocation, PermitRevocationViewDto>();
    }

    private void Reports()
    {
        CreateMap<Report, ReportUpdateDto>();
        CreateMap<Report, ReportViewDto>();
    }

    private void RmpInspections()
    {
        CreateMap<RmpInspection, InspectionUpdateDto>()
            .ForMember(dto => dto.InspectionStartedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionStarted.Date)))
            .ForMember(dto => dto.InspectionStartedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionStarted.TimeOfDay)))
            .ForMember(dto => dto.InspectionEndedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionEnded.Date)))
            .ForMember(dto => dto.InspectionEndedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionEnded.TimeOfDay)));
        CreateMap<RmpInspection, InspectionViewDto>();
    }

    private void SourceTestReviews()
    {
        CreateMap<SourceTestReview, SourceTestReviewUpdateDto>();
        CreateMap<SourceTestReview, SourceTestReviewViewDto>();
    }

    private void SearchResults()
    {
        CreateMap<WorkEntry, WorkEntrySearchResultDto>();
        CreateMap<Fce, FceSearchResultDto>();
    }
}
