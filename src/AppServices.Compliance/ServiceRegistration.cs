using AirWeb.AppServices.Compliance.Comments;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Compliance.Fces;
using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Compliance.SourceTests;
using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Compliance;

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
