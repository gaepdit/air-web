using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.RegisterServices;

public static class AppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services) => services
        // Work Entries
        .AddScoped<IWorkEntryManager, WorkEntryManager>()
        .AddScoped<IWorkEntryService, WorkEntryService>()
        .AddScoped<IWorkEntrySearchService, WorkEntrySearchService>()

        // FCEs
        .AddScoped<IFceManager, FceManager>()
        .AddScoped<IFceService, FceService>()
        .AddScoped<IFceSearchService, FceSearchService>()

        // Comments
        .AddScoped<ICommentService<int>, CommentService<int>>()

        // Notification Types
        .AddScoped<INotificationTypeManager, NotificationTypeManager>()
        .AddScoped<INotificationTypeService, NotificationTypeService>()

        // Enforcement
        .AddScoped<IEnforcementService, EnforcementService>()

        // Email
        .AddScoped<IAppNotificationService, AppNotificationService>()

        // Offices
        .AddScoped<IOfficeManager, OfficeManager>()
        .AddScoped<IOfficeService, OfficeService>();
}
