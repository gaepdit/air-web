using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.DataExport;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
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
