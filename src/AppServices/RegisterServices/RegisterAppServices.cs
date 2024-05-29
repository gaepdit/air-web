using AirWeb.AppServices.DataExport;
using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.Offices;
using AirWeb.AppServices.WorkEntries;
using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Entities.WorkEntries;
using GaEpd.EmailService;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.RegisterServices;

public static class RegisterAppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Work Entries
        services.AddScoped<IWorkEntryManager, WorkEntryManager>();
        services.AddScoped<IWorkEntryService, WorkEntryService>();

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
