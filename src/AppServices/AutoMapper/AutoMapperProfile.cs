using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.Facilities;
using AirWeb.AppServices.Fces;
using AirWeb.AppServices.Offices;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.AppServices.WorkEntries.Accs;
using AirWeb.AppServices.WorkEntries.Inspections;
using AirWeb.AppServices.WorkEntries.Notifications;
using AirWeb.AppServices.WorkEntries.PermitRevocations;
using AirWeb.AppServices.WorkEntries.Reports;
using AirWeb.AppServices.WorkEntries.RmpInspections;
using AirWeb.AppServices.WorkEntries.SourceTestReviews;
using AirWeb.Domain.Entities.EntryTypes;
using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;
using AutoMapper;

namespace AirWeb.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Users
        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();

        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();

        // Maintenance items
        CreateMap<EntryType, EntryTypeUpdateDto>();
        CreateMap<EntryType, EntryTypeViewDto>();

        // Facilities
        CreateMap<Facility, FacilityViewDto>();

        // FCEs
        CreateMap<Fce, FceUpdateDto>();
        CreateMap<Fce, FceViewDto>();

        // Work entries
        CreateMap<AnnualComplianceCertification, AccUpdateDto>();
        CreateMap<AnnualComplianceCertification, AccViewDto>()
            .ForMember(dto => dto.ClosedDate, expression =>
                expression.MapFrom<DateOnly?>(entry =>
                    entry.ClosedDate != null ? DateOnly.FromDateTime(entry.ClosedDate.Value.Date) : null));

        CreateMap<Inspection, InspectionUpdateDto>()
            .ForMember(dto => dto.InspectionStartedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionStarted.Date)))
            .ForMember(dto => dto.InspectionStartedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionStarted.TimeOfDay)))
            .ForMember(dto => dto.InspectionEndedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionEnded.Date)))
            .ForMember(dto => dto.InspectionEndedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionEnded.TimeOfDay)));
        CreateMap<Inspection, InspectionViewDto>()
            .ForMember(dto => dto.ClosedDate, expression =>
                expression.MapFrom<DateOnly?>(entry =>
                    entry.ClosedDate != null ? DateOnly.FromDateTime(entry.ClosedDate.Value.Date) : null));

        CreateMap<Notification, NotificationUpdateDto>();
        CreateMap<Notification, NotificationViewDto>()
            .ForMember(dto => dto.ComplianceEventType, expression => expression.Ignore())
            .ForMember(dto => dto.ClosedDate, expression =>
                expression.MapFrom<DateOnly?>(entry =>
                    entry.ClosedDate != null ? DateOnly.FromDateTime(entry.ClosedDate.Value.Date) : null));

        CreateMap<PermitRevocation, PermitRevocationUpdateDto>();
        CreateMap<PermitRevocation, PermitRevocationViewDto>()
            .ForMember(dto => dto.ComplianceEventType, expression => expression.Ignore())
            .ForMember(dto => dto.ClosedDate, expression =>
                expression.MapFrom<DateOnly?>(entry =>
                    entry.ClosedDate != null ? DateOnly.FromDateTime(entry.ClosedDate.Value.Date) : null));

        CreateMap<Report, ReportUpdateDto>();
        CreateMap<Report, ReportViewDto>()
            .ForMember(dto => dto.ClosedDate, expression =>
                expression.MapFrom<DateOnly?>(entry =>
                    entry.ClosedDate != null ? DateOnly.FromDateTime(entry.ClosedDate.Value.Date) : null));

        CreateMap<RmpInspection, RmpInspectionUpdateDto>()
            .ForMember(dto => dto.InspectionStartedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionStarted.Date)))
            .ForMember(dto => dto.InspectionStartedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionStarted.TimeOfDay)))
            .ForMember(dto => dto.InspectionEndedDate, expression =>
                expression.MapFrom(inspection => DateOnly.FromDateTime(inspection.InspectionEnded.Date)))
            .ForMember(dto => dto.InspectionEndedTime, expression =>
                expression.MapFrom(inspection => TimeOnly.FromTimeSpan(inspection.InspectionEnded.TimeOfDay)));
        CreateMap<RmpInspection, RmpInspectionViewDto>()
            .ForMember(dto => dto.ClosedDate, expression =>
                expression.MapFrom<DateOnly?>(entry =>
                    entry.ClosedDate != null ? DateOnly.FromDateTime(entry.ClosedDate.Value.Date) : null));

        CreateMap<SourceTestReview, SourceTestReviewUpdateDto>();
        CreateMap<SourceTestReview, SourceTestReviewViewDto>()
            .ForMember(dto => dto.ClosedDate, expression =>
                expression.MapFrom<DateOnly?>(entry =>
                    entry.ClosedDate != null ? DateOnly.FromDateTime(entry.ClosedDate.Value.Date) : null));
    }
}
