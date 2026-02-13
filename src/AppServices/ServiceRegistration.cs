using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.SourceTests;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices;

public static class ServiceRegistration
{
    public static IServiceCollection AddComplianceAppServices(this IServiceCollection services) => services

        // Compliance Work
        .AddScoped<IComplianceWorkManager, ComplianceWorkManager>()
        .AddScoped<IComplianceWorkService, ComplianceWorkService>()
        .AddScoped<IComplianceWorkSearchService, ComplianceWorkSearchService>()

        // Source Tests
        .AddScoped<ISourceTestAppService, SourceTestAppService>()

        // FCEs
        .AddScoped<IFceManager, FceManager>()
        .AddScoped<IFceService, FceService>()
        .AddScoped<IFceSearchService, FceSearchService>()

        // Comments
        .AddScoped<ICaseFileCommentService, CaseFileCommentService>()
        .AddScoped<IComplianceWorkCommentService, ComplianceWorkCommentService>()
        .AddScoped<IFceCommentService, FceCommentService>()

        // Notification Types
        .AddScoped<INotificationTypeManager, NotificationTypeManager>()
        .AddScoped<INotificationTypeService, NotificationTypeService>()

        // Enforcement
        .AddScoped<ICaseFileSearchService, CaseFileSearchService>()
        .AddScoped<ICaseFileService, CaseFileService>()
        .AddScoped<ICaseFileManager, CaseFileManager>()
        .AddScoped<IEnforcementActionService, EnforcementActionService>()
        .AddScoped<IEnforcementActionManager, EnforcementActionManager>()

        // Validators
        .AddValidatorsFromAssemblyContaining(typeof(ServiceRegistration));
}
