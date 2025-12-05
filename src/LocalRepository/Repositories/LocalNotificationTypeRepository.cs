using AirWeb.Domain.Lookups.NotificationTypes;
using AirWeb.TestData.Lookups;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalNotificationTypeRepository()
    : NamedEntityRepository<NotificationType>(NotificationTypeData.GetData),
        INotificationTypeRepository;
