using AutoMapper;
using MyApp.AppServices.EntryActions.Dto;
using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.Offices;
using MyApp.AppServices.Staff.Dto;
using MyApp.AppServices.WorkEntries.CommandDto;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.Domain.Entities.EntryActions;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.Offices;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.Domain.Identity;

namespace MyApp.AppServices.AutoMapper;

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

        CreateMap<WorkEntry, WorkEntrySearchResultDto>();
        CreateMap<WorkEntry, WorkEntryCreateDto>();
        CreateMap<WorkEntry, WorkEntryViewDto>();
    }
}
