using AirWeb.Domain.Core;

namespace AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;

public class NotificationType : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public NotificationType() { }
    internal NotificationType(Guid id, string name) : base(id, name) { }
}

public interface INotificationTypeRepository : INamedEntityRepository<NotificationType>;

public interface INotificationTypeManager : INamedEntityManager<NotificationType>;

public class NotificationTypeManager(INotificationTypeRepository repository)
    : NamedEntityManager<NotificationType, INotificationTypeRepository>(repository), INotificationTypeManager;
