using AirWeb.AppServices.AuditPoints;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Fces.SupportingData;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Enforcement.CaseFileQuery;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using AirWeb.Domain.Lookups.NotificationTypes;
using AirWeb.Domain.Lookups.Offices;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.AutoMapper;

public static class AutoMapperProfileRegistration
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services) =>
        services.AddAutoMapper(expression => expression.AddProfile<AutoMapperProfile>());
}

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        Users();
        MaintenanceItems();
        Comments();
        AuditPoints();
        Fces();
        WorkEntries();
        Enforcement();
    }

    private void WorkEntries()
    {
        CreateMap<WorkEntry, WorkEntrySummaryDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<WorkEntry, WorkEntrySearchResultDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());

        Accs();
        Inspections();
        Notifications();
        PermitRevocations();
        Reports();
        RmpInspections();
        SourceTestReviews();
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

    private void AuditPoints()
    {
        CreateMap<AuditPoint, AuditPointViewDto>();
    }

    private void Fces()
    {
        CreateMap<Fce, FceUpdateDto>();
        CreateMap<Fce, FceSummaryDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<Fce, FceViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<Fce, FceSearchResultDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());

        // Supporting data
        CreateMap<AnnualComplianceCertification, AccSummaryDto>();
        CreateMap<Inspection, InspectionSummaryDto>();
        CreateMap<Notification, NotificationSummaryDto>();
        CreateMap<Report, ReportSummaryDto>();
        CreateMap<RmpInspection, InspectionSummaryDto>();
        CreateMap<CaseFile, EnforcementCaseSummaryDto>();
    }

    private void Accs()
    {
        CreateMap<AccViewDto, AccUpdateDto>();
        CreateMap<AnnualComplianceCertification, AccViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void Inspections()
    {
        CreateMap<InspectionViewDto, InspectionUpdateDto>()
            .ForMember(dto => dto.InspectionStartedDate, expression =>
                expression.MapFrom(dto => DateOnly.FromDateTime(dto.InspectionStarted.Date)))
            .ForMember(dto => dto.InspectionStartedTime, expression =>
                expression.MapFrom(dto => TimeOnly.FromTimeSpan(dto.InspectionStarted.TimeOfDay)))
            .ForMember(dto => dto.InspectionEndedDate, expression =>
                expression.MapFrom(dto => DateOnly.FromDateTime(dto.InspectionEnded.Date)))
            .ForMember(dto => dto.InspectionEndedTime, expression =>
                expression.MapFrom(dto => TimeOnly.FromTimeSpan(dto.InspectionEnded.TimeOfDay)));
        CreateMap<Inspection, InspectionViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void Notifications()
    {
        CreateMap<NotificationViewDto, NotificationUpdateDto>();
        CreateMap<Notification, NotificationViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void PermitRevocations()
    {
        CreateMap<PermitRevocationViewDto, PermitRevocationUpdateDto>();
        CreateMap<PermitRevocation, PermitRevocationViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void Reports()
    {
        CreateMap<ReportViewDto, ReportUpdateDto>();
        CreateMap<Report, ReportViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void RmpInspections()
    {
        // InspectionUpdateDto is handled in Inspections()

        CreateMap<RmpInspection, InspectionViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
    }

    private void SourceTestReviews()
    {
        CreateMap<SourceTestReviewViewDto, SourceTestReviewUpdateDto>();
        CreateMap<SourceTestReview, SourceTestReviewViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<SourceTestReview, SourceTestSummaryDto>()
            .ForMember(dto => dto.ComplianceStatus, expression => expression.Ignore())
            .ForMember(dto => dto.PollutantMeasured, expression => expression.Ignore())
            .ForMember(dto => dto.SourceTested, expression => expression.Ignore());
    }

    private void Enforcement()
    {
        CreateMap<CaseFile, CaseFileViewDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore())
            .ForMember(dto => dto.EnforcementActions, expression => expression.Ignore())
            .ForMember(dto => dto.AirProgramsAsStrings, expression => expression.Ignore());
        CreateMap<CaseFile, CaseFileSummaryDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());
        CreateMap<CaseFile, CaseFileSearchResultDto>()
            .ForMember(dto => dto.FacilityName, expression => expression.Ignore());

        CreateMap<CaseFileViewDto, CaseFileSummaryDto>();

        CreateMap<EnforcementAction, ActionViewDto>();
        CreateMap<EnforcementAction, ActionTypeDto>();
        CreateMap<EnforcementActionReview, ReviewDto>();

        CreateMap<AdministrativeOrder, AoViewDto>();
        CreateMap<AoViewDto, AdministrativeOrderCommandDto>()
            .ForMember(dto => dto.Comment, expression => expression.MapFrom(dto => dto.Notes));
        CreateMap<AdministrativeOrderCommandDto, AdministrativeOrder>(MemberList.Source)
            .ForMember(dto => dto.Notes, expression => expression.MapFrom(dto => dto.Comment));

        CreateMap<ConsentOrder, CoViewDto>();
        CreateMap<CoViewDto, ConsentOrderCommandDto>()
            .ForMember(dto => dto.Comment, expression => expression.MapFrom(dto => dto.Notes));
        CreateMap<ConsentOrderCommandDto, ConsentOrder>(MemberList.Source)
            .ForMember(dto => dto.Notes, expression => expression.MapFrom(dto => dto.Comment));

        CreateMap<InformationalLetter, ResponseRequestedViewDto>();
        CreateMap<LetterOfNoncompliance, LonViewDto>();

        CreateMap<NoFurtherActionLetter, ActionViewDto>();
        CreateMap<NoticeOfViolation, ResponseRequestedViewDto>();
        CreateMap<NovNfaLetter, ResponseRequestedViewDto>();
        CreateMap<ProposedConsentOrder, ProposedCoViewDto>();

        CreateMap<StipulatedPenalty, StipulatedPenaltyViewDto>();

        CreateMap<ResponseRequestedViewDto, EnforcementActionCreateDto>()
            .ForMember(dto => dto.Comment, expression => expression.MapFrom(dto => dto.Notes));
    }
}
