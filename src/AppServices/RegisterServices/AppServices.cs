using AirWeb.AppServices.DataExport;
using AirWeb.AppServices.DomainEntities.Fces;
using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.AppServices.DomainEntities.Offices;
using AirWeb.AppServices.DomainEntities.WorkEntries;
using AirWeb.AppServices.Notifications;
using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using GaEpd.EmailService;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.RegisterServices;

public static class AppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Work Entries
        services.AddScoped<IWorkEntryManager, WorkEntryManager>();
        services.AddScoped<IWorkEntryService, WorkEntryService>();

        // FCEs
        services.AddScoped<IFceManager, FceManager>();
        services.AddScoped<IFceService, FceService>();

        // Notification Types
        services.AddScoped<INotificationTypeManager, NotificationTypeManager>();
        services.AddScoped<INotificationTypeService, NotificationTypeService>();

        // Email
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeService, OfficeService>();

        // Data Export
        services.AddScoped<ISearchResultsExportService, SearchResultsExportService>();
    }
}
