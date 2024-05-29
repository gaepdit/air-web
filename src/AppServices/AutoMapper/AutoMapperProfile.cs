using AirWeb.AppServices.EntryActions.Dto;
using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.Offices;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.AppServices.WorkEntries.Accs;
using AirWeb.AppServices.WorkEntries.Inspections;
using AirWeb.AppServices.WorkEntries.Notifications;
using AirWeb.AppServices.WorkEntries.PermitRevocations;
using AirWeb.AppServices.WorkEntries.Reports;
using AirWeb.AppServices.WorkEntries.RmpInspections;
using AirWeb.AppServices.WorkEntries.SourceTestReviews;
using AirWeb.Domain.Entities.EntryActions;
using AirWeb.Domain.Entities.EntryTypes;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;
using AutoMapper;

namespace AirWeb.AppServices.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();

        CreateMap<EntryAction, EntryActionUpdateDto>();
        CreateMap<EntryAction, EntryActionViewDto>();

        CreateMap<EntryType, EntryTypeUpdateDto>();
        CreateMap<EntryType, EntryTypeViewDto>();

        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();

        // Work entries
        CreateMap<AnnualComplianceCertification, AccCreateDto>();
        CreateMap<AnnualComplianceCertification, AccUpdateDto>();
        CreateMap<AnnualComplianceCertification, AccViewDto>();

        CreateMap<Inspection, InspectionCreateDto>();
        CreateMap<Inspection, InspectionUpdateDto>();
        CreateMap<Inspection, InspectionViewDto>();

        CreateMap<Notification, NotificationCreateDto>();
        CreateMap<Notification, NotificationUpdateDto>();
        CreateMap<Notification, NotificationViewDto>();

        CreateMap<PermitRevocation, PermitRevocationCreateDto>();
        CreateMap<PermitRevocation, PermitRevocationUpdateDto>();
        CreateMap<PermitRevocation, PermitRevocationViewDto>();

        CreateMap<Report, ReportCreateDto>();
        CreateMap<Report, ReportUpdateDto>();
        CreateMap<Report, ReportViewDto>();

        CreateMap<RmpInspection, RmpInspectionCreateDto>();
        CreateMap<RmpInspection, RmpInspectionUpdateDto>();
        CreateMap<RmpInspection, RmpInspectionViewDto>();

        CreateMap<SourceTestReview, SourceTestReviewCreateDto>();
        CreateMap<SourceTestReview, SourceTestReviewUpdateDto>();
        CreateMap<SourceTestReview, SourceTestReviewViewDto>();
    }
}
