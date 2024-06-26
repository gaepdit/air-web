using AirWeb.AppServices.DomainEntities.Facilities;
using AirWeb.AppServices.DomainEntities.Fces;
using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.AppServices.DomainEntities.Offices;
using AirWeb.AppServices.DomainEntities.WorkEntries.Accs;
using AirWeb.AppServices.DomainEntities.WorkEntries.Inspections;
using AirWeb.AppServices.DomainEntities.WorkEntries.Notifications;
using AirWeb.AppServices.DomainEntities.WorkEntries.PermitRevocations;
using AirWeb.AppServices.DomainEntities.WorkEntries.Reports;
using AirWeb.AppServices.DomainEntities.WorkEntries.RmpInspections;
using AirWeb.AppServices.DomainEntities.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
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
