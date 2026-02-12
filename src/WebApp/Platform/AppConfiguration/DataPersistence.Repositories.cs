using AirWeb.Domain.Comments;
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
    extension(IServiceCollection services)
    {
        private void AddEntityFrameworkRepositories() => services
            .AddScoped<IEmailLogRepository, EmailLogRepository>()
            .AddScoped<INotificationTypeRepository, NotificationTypeRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>()
            .AddScoped<IFceRepository, FceRepository>()
            .AddScoped<IComplianceWorkRepository, ComplianceWorkRepository>()
            .AddScoped<IEnforcementActionRepository, EnforcementActionRepository>()
            .AddScoped<ICaseFileRepository, CaseFileRepository>()
            .AddScoped<ICaseFileCommentRepository, CaseFileCommentRepository>()
            .AddScoped<IComplianceWorkCommentRepository, ComplianceWorkCommentRepository>()
            .AddScoped<IFceCommentRepository, FceCommentRepository>();

        private void AddInMemoryRepositories() => services
            .AddSingleton<IEmailLogRepository, LocalEmailLogRepository>()
            .AddSingleton<INotificationTypeRepository, LocalNotificationTypeRepository>()
            .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
            .AddSingleton<IFceRepository, LocalFceRepository>()
            .AddSingleton<IComplianceWorkRepository, LocalComplianceWorkRepository>()
            .AddSingleton<IEnforcementActionRepository, LocalEnforcementActionRepository>()
            .AddSingleton<ICaseFileRepository, LocalCaseFileRepository>()
            .AddSingleton<ICaseFileCommentRepository, LocalCaseFileCommentRepository>()
            .AddSingleton<IComplianceWorkCommentRepository, LocalComplianceWorkCommentRepository>()
            .AddSingleton<IFceCommentRepository, LocalFceCommentRepository>();
    }
}
