using AutoMapper;
using AirWeb.AppServices.EntryActions.Dto;
using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.Offices;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.AppServices.WorkEntries.CommandDto;
using AirWeb.AppServices.WorkEntries.SearchDto;
using AirWeb.AppServices.WorkEntries.ViewDto;
using AirWeb.Domain.Entities.EntryActions;
using AirWeb.Domain.Entities.EntryTypes;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;

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

        CreateMap<WorkEntry, WorkEntrySearchResultDto>();
        CreateMap<WorkEntry, BaseWorkEntryCreateDto>();
        CreateMap<WorkEntry, BaseWorkEntryViewDto>();
    }
}
