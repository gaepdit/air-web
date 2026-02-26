using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.ValueObjects;
using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AirWeb.Domain.Sbeap.Entities.Cases;
using AirWeb.Domain.Sbeap.Entities.Contacts;
using AirWeb.Domain.Sbeap.Entities.Customers;
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
        CreateMap<ActionItem, ActionItemViewDto>();
        CreateMap<ActionItem, ActionItemUpdateDto>();

        // Action Item Types
        CreateMap<ActionItemType, ActionItemTypeViewDto>();
        CreateMap<ActionItemType, ActionItemTypeUpdateDto>();

        // Agencies
        CreateMap<Agency, AgencyViewDto>();
        CreateMap<Agency, AgencyUpdateDto>();

        // Cases
        CreateMap<Casework, CaseworkViewDto>()
            .ForMember(dto => dto.DeletedBy, expression => expression.Ignore());
        CreateMap<Casework, CaseworkUpdateDto>();
        CreateMap<Casework, CaseworkSearchResultDto>();

        // Contacts
        CreateMap<Contact, ContactViewDto>();
        CreateMap<Contact, ContactUpdateDto>();

        // Customers
        CreateMap<Customer, CustomerViewDto>()
            .ForMember(dto => dto.DeletedBy, expression => expression.Ignore());
        CreateMap<Customer, CustomerUpdateDto>();
        CreateMap<Customer, CustomerSearchResultDto>();

        // Address
        CreateMap<Address, OptionalAddress>();
    }
}
