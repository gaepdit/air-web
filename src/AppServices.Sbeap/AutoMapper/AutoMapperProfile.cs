using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Sbeap.AutoMapper;

public static class ProfileRegistration
{
    public static IServiceCollection AddSbeapAutoMapperProfiles(this IServiceCollection services) =>
        services.AddAutoMapper(expression => expression.AddProfile<AutoMapperProfile>());
}

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Action Items
        // CreateMap<ActionItem, ActionItemViewDto>();
        // CreateMap<ActionItem, ActionItemUpdateDto>();

        // Action Item Types
        CreateMap<ActionItemType, ActionItemTypeViewDto>();
        CreateMap<ActionItemType, ActionItemTypeUpdateDto>();

        // Agencies
        // CreateMap<Agency, AgencyViewDto>();
        // CreateMap<Agency, AgencyUpdateDto>();

        // Cases
        // CreateMap<Casework, CaseworkViewDto>()
        // .ForMember(dto => dto.DeletedBy, expression => expression.Ignore());
        // CreateMap<Casework, CaseworkUpdateDto>();
        // CreateMap<Casework, CaseworkSearchResultDto>();

        // Contacts
        // CreateMap<Contact, ContactViewDto>();
        // CreateMap<Contact, ContactUpdateDto>();

        // Customers
        // CreateMap<Customer, CustomerViewDto>()
        // .ForMember(dto => dto.DeletedBy, expression => expression.Ignore());
        // CreateMap<Customer, CustomerUpdateDto>();
        // CreateMap<Customer, CustomerSearchResultDto>();
    }
}
