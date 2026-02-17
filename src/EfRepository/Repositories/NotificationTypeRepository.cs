using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.Repositories;

public sealed class NotificationTypeRepository(AppDbContext context)
    : NamedEntityRepository<NotificationType, AppDbContext>(context),
        INotificationTypeRepository;
