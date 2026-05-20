using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;

namespace AirWeb.MemRepository.ComplianceRepositories;

public sealed class NotificationTypeMemRepository()
    : NamedEntityRepository<NotificationType>(NotificationTypeData.GetData),
        INotificationTypeRepository;
