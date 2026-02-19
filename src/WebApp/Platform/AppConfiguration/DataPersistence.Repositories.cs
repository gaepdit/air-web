using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Core.Entities;
using AirWeb.EfRepository.Repositories;
using AirWeb.MemRepository.CommonRepositories;
using AirWeb.MemRepository.ComplianceRepositories;

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
            .AddSingleton<IEmailLogRepository, EmailLogMemRepository>()
            .AddSingleton<INotificationTypeRepository, NotificationTypeMemRepository>()
            .AddSingleton<IOfficeRepository, OfficeMemRepository>()
            .AddSingleton<IFceRepository, FceMemRepository>()
            .AddSingleton<IComplianceWorkRepository, ComplianceWorkMemRepository>()
            .AddSingleton<IEnforcementActionRepository, EnforcementActionMemRepository>()
            .AddSingleton<ICaseFileRepository, CaseFileMemRepository>()
            .AddSingleton<ICaseFileCommentRepository, CaseFileCommentMemRepository>()
            .AddSingleton<IComplianceWorkCommentRepository, ComplianceWorkCommentMemRepository>()
            .AddSingleton<IFceCommentRepository, FceCommentMemRepository>();
    }
}
