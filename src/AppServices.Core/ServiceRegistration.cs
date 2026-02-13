using AirWeb.AppServices.Core.AppNotifications;
using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Staff;
using AirWeb.Core.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Core;

public static class ServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services) => services

        // Email
        .AddScoped<IAppNotificationService, AppNotificationService>()

        // Offices
        .AddScoped<IOfficeManager, OfficeManager>()
        .AddScoped<IOfficeService, OfficeService>()

        // Staff
        .AddScoped<IStaffService, StaffService>()

        // Validators
        .AddValidatorsFromAssemblyContaining(typeof(ServiceRegistration));
}
