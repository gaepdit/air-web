namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;

public interface INotificationCommandDto
{
    public DateOnly ReceivedDate { get; }
    public DateOnly? DueDate { get; }
    public DateOnly? SentDate { get; }
    public Guid? NotificationTypeId { get; }
    public bool FollowupTaken { get; }
}
