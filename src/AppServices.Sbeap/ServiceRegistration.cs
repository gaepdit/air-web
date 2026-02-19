using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.AppServices.Sbeap.Cases;
using AirWeb.AppServices.Sbeap.Customers;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AirWeb.Domain.Sbeap.Entities.Cases;
using AirWeb.Domain.Sbeap.Entities.Customers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Sbeap;

public static class ServiceRegistration
{
    public static IServiceCollection AddSbeapAppServices(this IServiceCollection services) => services

        // Agencies
        .AddScoped<IAgencyService, AgencyService>()
        .AddScoped<IAgencyManager, AgencyManager>()

        // Action Item Types
        .AddScoped<IActionItemTypeService, ActionItemTypeService>()
        .AddScoped<IActionItemTypeManager, ActionItemTypeManager>()

        // Action Items
        .AddScoped<IActionItemService, ActionItemService>()

        // Cases
        .AddScoped<ICaseworkService, CaseworkService>()
        .AddScoped<ICaseworkManager, CaseworkManager>()

        // Customers/Contacts
        .AddScoped<ICustomerService, CustomerService>()
        .AddScoped<ICustomerManager, CustomerManager>()

        // Validators
        .AddValidatorsFromAssemblyContaining(typeof(ServiceRegistration));
}
