using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Enforcement.Query;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.ActionProperties;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using AutoMapper;

namespace AirWeb.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        Users();
        MaintenanceItems();
        Comments();
        Fces();
        WorkEntries();
        Accs();
        Inspections();
        Notifications();
        PermitRevocations();
        Reports();
        RmpInspections();
        SourceTestReviews();
        SearchResults();
        Enforcement();
    }

    private void WorkEntries()
    {
        CreateMap<WorkEntry, WorkEntrySummaryDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
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

    private void Fces()
    {
        CreateMap<Fce, FceUpdateDto>();
        CreateMap<Fce, FceSummaryDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<Fce, FceViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void Accs()
    {
        CreateMap<AnnualComplianceCertification, AccUpdateDto>();
        CreateMap<AnnualComplianceCertification, AccViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
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
        CreateMap<Inspection, InspectionViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void Notifications()
    {
        CreateMap<Notification, NotificationUpdateDto>();
        CreateMap<Notification, NotificationViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void PermitRevocations()
    {
        CreateMap<PermitRevocation, PermitRevocationUpdateDto>();
        CreateMap<PermitRevocation, PermitRevocationViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void Reports()
    {
        CreateMap<Report, ReportUpdateDto>();
        CreateMap<Report, ReportViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
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
        CreateMap<RmpInspection, InspectionViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void SourceTestReviews()
    {
        CreateMap<SourceTestReview, SourceTestReviewUpdateDto>();
        CreateMap<SourceTestReview, SourceTestReviewViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void SearchResults()
    {
        CreateMap<WorkEntry, WorkEntrySearchResultDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<Fce, FceSearchResultDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void Enforcement()
    {
        CreateMap<CaseFile, CaseFileViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<CaseFile, CaseFileSummaryDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<EnforcementAction, EnforcementActionViewDto>();
        CreateMap<EnforcementActionReview, ReviewDto>();
    }
}
