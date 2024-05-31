namespace AirWeb.Domain.Entities.NotificationTypes;

public class NotificationTypeManager(INotificationTypeRepository repository)
    : NamedEntityManager<NotificationType, INotificationTypeRepository>(repository), INotificationTypeManager;
