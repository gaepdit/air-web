using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.RmpInspections;
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
using AutoMapper;

namespace AirWeb.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMapsForUsers();
        CreateMapsForMaintenanceItems();
        CreateMapsForFacilities();
        CreateMapsForFces();
        CreateMapsForAccs();
        CreateMapsForInspections();
        CreateMapsForNotifications();
        CreateMapsForPermitRevocations();
        CreateMapsForReports();
        CreateMapsForRmpInspections();
        CreateMapsForSourceTestReviews();
    }

    private void CreateMapsForUsers()
    {
        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();
    }

    private void CreateMapsForMaintenanceItems()
    {
        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();

        CreateMap<NotificationType, NotificationTypeUpdateDto>();
        CreateMap<NotificationType, NotificationTypeViewDto>();
    }

    private void CreateMapsForFacilities()
    {
        CreateMap<Facility, FacilityViewDto>();
    }

    private void CreateMapsForFces()
    {
        CreateMap<Fce, FceUpdateDto>();
        CreateMap<Fce, FceViewDto>();
    }

    private void CreateMapsForAccs()
    {
        CreateMap<AnnualComplianceCertification, AccUpdateDto>();
        CreateMap<AnnualComplianceCertification, AccViewDto>();
    }

    private void CreateMapsForInspections()
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

    private void CreateMapsForNotifications()
    {
        CreateMap<Notification, NotificationUpdateDto>();
        CreateMap<Notification, NotificationViewDto>()
            .ForMember(dto => dto.ComplianceEventType, expression => expression.Ignore());
    }

    private void CreateMapsForPermitRevocations()
    {
        CreateMap<PermitRevocation, PermitRevocationUpdateDto>();
        CreateMap<PermitRevocation, PermitRevocationViewDto>()
            .ForMember(dto => dto.ComplianceEventType, expression => expression.Ignore());
    }

    private void CreateMapsForReports()
    {
        CreateMap<Report, ReportUpdateDto>();
        CreateMap<Report, ReportViewDto>();
    }

    private void CreateMapsForRmpInspections()
    {
        CreateMap<RmpInspection, RmpInspectionUpdateDto>()
            .ForMember(dto => dto.InspectionStartedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionStarted.Date)))
            .ForMember(dto => dto.InspectionStartedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionStarted.TimeOfDay)))
            .ForMember(dto => dto.InspectionEndedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionEnded.Date)))
            .ForMember(dto => dto.InspectionEndedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionEnded.TimeOfDay)));
        CreateMap<RmpInspection, RmpInspectionViewDto>();
    }

    private void CreateMapsForSourceTestReviews()
    {
        CreateMap<SourceTestReview, SourceTestReviewUpdateDto>();
        CreateMap<SourceTestReview, SourceTestReviewViewDto>();
    }
}
