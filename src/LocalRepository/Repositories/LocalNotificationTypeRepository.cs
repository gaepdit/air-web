using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.TestData;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalNotificationTypeRepository()
    : NamedEntityRepository<NotificationType>(EntryTypeData.GetData), INotificationTypeRepository;
