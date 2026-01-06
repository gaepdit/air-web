using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.EmailLog;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Lookups.NotificationTypes;
using AirWeb.Domain.Lookups.Offices;
using AirWeb.EfRepository.Repositories;
using AirWeb.LocalRepository.Repositories;

namespace AirWeb.WebApp.Platform.AppConfiguration;

internal static partial class DataPersistence
{
    private static void AddEntityFrameworkRepositories(this IServiceCollection services) => services
        .AddScoped<IEmailLogRepository, EmailLogRepository>()
        .AddScoped<INotificationTypeRepository, NotificationTypeRepository>()
        .AddScoped<IOfficeRepository, OfficeRepository>()
        .AddScoped<IFceRepository, FceRepository>()
        .AddScoped<IComplianceWorkRepository, ComplianceWorkRepository>()
        .AddScoped<IEnforcementActionRepository, EnforcementActionRepository>()
        .AddScoped<ICaseFileRepository, CaseFileRepository>();

    private static void AddInMemoryRepositories(this IServiceCollection services) => services
        .AddSingleton<IEmailLogRepository, LocalEmailLogRepository>()
        .AddSingleton<INotificationTypeRepository, LocalNotificationTypeRepository>()
        .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
        .AddSingleton<IFceRepository, LocalFceRepository>()
        .AddSingleton<IComplianceWorkRepository, LocalComplianceWorkRepository>()
        .AddSingleton<IEnforcementActionRepository, LocalEnforcementActionRepository>()
        .AddSingleton<ICaseFileRepository, LocalCaseFileRepository>();
}
