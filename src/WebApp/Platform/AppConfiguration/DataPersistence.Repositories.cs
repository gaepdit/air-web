using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EmailLog;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.Offices;
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
        .AddScoped<IWorkEntryRepository, WorkEntryRepository>()
        .AddScoped<IEnforcementActionRepository, EnforcementActionRepository>()
        .AddScoped<ICaseFileRepository, CaseFileRepository>();

    private static void AddInMemoryRepositories(this IServiceCollection services) => services
        .AddSingleton<IEmailLogRepository, LocalEmailLogRepository>()
        .AddSingleton<INotificationTypeRepository, LocalNotificationTypeRepository>()
        .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
        .AddSingleton<IFceRepository, LocalFceRepository>()
        .AddSingleton<IWorkEntryRepository, LocalWorkEntryRepository>()
        .AddSingleton<IEnforcementActionRepository, LocalEnforcementActionRepository>()
        .AddSingleton<ICaseFileRepository, LocalCaseFileRepository>();
}
