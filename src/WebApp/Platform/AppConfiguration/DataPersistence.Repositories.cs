using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AirWeb.Domain.Sbeap.Entities.Cases;
using AirWeb.Domain.Sbeap.Entities.Contacts;
using AirWeb.Domain.Sbeap.Entities.Customers;
using AirWeb.EfRepository.Repositories;
using AirWeb.MemRepository.CommonRepositories;
using AirWeb.MemRepository.ComplianceRepositories;
using AirWeb.MemRepository.SbeapRepositories;

namespace AirWeb.WebApp.Platform.AppConfiguration;

internal static partial class DataPersistence
{
    extension(IServiceCollection services)
    {
        private void AddEntityFrameworkRepositories() => services
            // Common
            .AddScoped<IEmailLogRepository, EmailLogRepository>()
            .AddScoped<INotificationTypeRepository, NotificationTypeRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>()

            // Compliance/enforcent
            .AddScoped<IFceRepository, FceRepository>()
            .AddScoped<IComplianceWorkRepository, ComplianceWorkRepository>()
            .AddScoped<IEnforcementActionRepository, EnforcementActionRepository>()
            .AddScoped<ICaseFileRepository, CaseFileRepository>()
            .AddScoped<ICaseFileCommentRepository, CaseFileCommentRepository>()
            .AddScoped<IComplianceWorkCommentRepository, ComplianceWorkCommentRepository>()
            .AddScoped<IFceCommentRepository, FceCommentRepository>()

        // SBEAP
        // .AddScoped<IActionItemRepository, ActionItemRepository>()
        // .AddScoped<IActionItemTypeRepository, ActionItemTypeRepository>()
        // .AddScoped<IAgencyRepository, AgencyRepository>()
        // .AddScoped<ICaseworkRepository, CaseworkRepository>()
        // .AddScoped<IContactRepository, ContactRepository>()
        // .AddScoped<ICustomerRepository, CustomerRepository>()
        ;

        private void AddInMemoryRepositories() => services
            // Common
            .AddSingleton<IEmailLogRepository, EmailLogMemRepository>()
            .AddSingleton<INotificationTypeRepository, NotificationTypeMemRepository>()
            .AddSingleton<IOfficeRepository, OfficeMemRepository>()

            // Compliance/enforcent
            .AddSingleton<IFceRepository, FceMemRepository>()
            .AddSingleton<IComplianceWorkRepository, ComplianceWorkMemRepository>()
            .AddSingleton<IEnforcementActionRepository, EnforcementActionMemRepository>()
            .AddSingleton<ICaseFileRepository, CaseFileMemRepository>()
            .AddSingleton<ICaseFileCommentRepository, CaseFileCommentMemRepository>()
            .AddSingleton<IComplianceWorkCommentRepository, ComplianceWorkCommentMemRepository>()
            .AddSingleton<IFceCommentRepository, FceCommentMemRepository>()

            // SBEAP
            .AddSingleton<IActionItemRepository, ActionItemMemRepository>()
            .AddSingleton<IActionItemTypeRepository, ActionItemTypeMemRepository>()
            .AddSingleton<IAgencyRepository, AgencyMemRepository>()
            .AddSingleton<ICaseworkRepository, CaseworkMemRepository>()
            .AddSingleton<IContactRepository, ContactMemRepository>()
            .AddSingleton<ICustomerRepository, CustomerMemRepository>();
    }
}
