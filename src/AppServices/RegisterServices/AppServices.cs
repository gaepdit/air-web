using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.DataExport;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.NamedEntities.Offices;
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
        // Facilities
        services.AddScoped<IFacilityService, FacilityService>();

        // Work Entries
        services.AddScoped<IWorkEntryManager, WorkEntryManager>();
        services.AddScoped<IWorkEntryService, WorkEntryService>();

        // FCEs
        services.AddScoped<IFceManager, FceManager>();
        services.AddScoped<IFceService, FceService>();

        // Compliance search
        services.AddScoped<IComplianceSearchService, ComplianceSearchService>();

        // Notification Types
        services.AddScoped<INotificationTypeManager, NotificationTypeManager>();
        services.AddScoped<INotificationTypeService, NotificationTypeService>();

        // Email
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IAppNotificationService, AppNotificationService>();

        // Offices
        services.AddScoped<IOfficeManager, OfficeManager>();
        services.AddScoped<IOfficeService, OfficeService>();

        // Data Export
        services.AddScoped<ISearchResultsExportService, SearchResultsExportService>();
    }
}
