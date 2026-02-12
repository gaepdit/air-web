using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Lookups;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalNotificationTypeRepository()
    : NamedEntityRepository<NotificationType>(NotificationTypeData.GetData),
        INotificationTypeRepository;
