namespace AirWeb.Domain.Entities.NotificationTypes;

public class NotificationType : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public NotificationType() { }
    internal NotificationType(Guid id, string name) : base(id, name) { }
}
