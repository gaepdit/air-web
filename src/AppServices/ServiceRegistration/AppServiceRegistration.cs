using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.ComplianceWork;
using AirWeb.AppServices.Compliance.ComplianceWork.Search;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.SourceTests;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.AppServices.Lookups.Offices;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Lookups.NotificationTypes;
using AirWeb.Domain.Lookups.Offices;
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
        .AddScoped<ISourceTestAppService, SourceTestAppService>()

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
