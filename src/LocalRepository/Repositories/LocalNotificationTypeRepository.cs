using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.TestData.Entities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalNotificationTypeRepository()
    : NamedEntityRepository<NotificationType>(NotificationTypeData.GetData), INotificationTypeRepository;
