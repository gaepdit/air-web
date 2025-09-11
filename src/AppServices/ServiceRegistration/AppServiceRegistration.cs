using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.SourceTests;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.ServiceRegistration;

public static class AppServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services) => services
        // Work Entries
        .AddScoped<IWorkEntryManager, WorkEntryManager>()
        .AddScoped<IWorkEntryService, WorkEntryService>()
        .AddScoped<IWorkEntrySearchService, WorkEntrySearchService>()

        // Source Tests
        .AddScoped<ISourceTestsService, SourceTestsService>()

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
        .AddScoped<ICaseFileSearchService, CaseFileSearchService>()
        .AddScoped<ICaseFileService, CaseFileService>()
        .AddScoped<ICaseFileManager, CaseFileManager>()
        .AddScoped<IEnforcementActionService, EnforcementActionService>()
        .AddScoped<IEnforcementActionManager, EnforcementActionManager>()

        // Email
        .AddScoped<IAppNotificationService, AppNotificationService>()

        // Offices
        .AddScoped<IOfficeManager, OfficeManager>()
        .AddScoped<IOfficeService, OfficeService>()

        // Staff
        .AddScoped<IStaffService, StaffService>()

        // Validators
        .AddValidatorsFromAssemblyContaining(typeof(AppServiceRegistration));
}
