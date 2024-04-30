using GaEpd.EmailService;
using Microsoft.Extensions.DependencyInjection;
using MyApp.AppServices.DataExport;
using MyApp.AppServices.EntryActions;
using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.Notifications;
using MyApp.AppServices.Offices;
using MyApp.AppServices.WorkEntries;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.Offices;
using MyApp.Domain.Entities.WorkEntries;

namespace MyApp.AppServices.RegisterServices;

public static class RegisterAppServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Work Entries
        services.AddScoped<IWorkEntryManager, WorkEntryManager>();
        services.AddScoped<IWorkEntryService, WorkEntryService>();

        // Entry Actions
        services.AddScoped<IEntryActionService, EntryActionService>();

        // Entry Types
        services.AddScoped<IEntryTypeManager, EntryTypeManager>();
        services.AddScoped<IEntryTypeService, EntryTypeService>();
        
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
