using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.AppServices.WorkEntries.Notifications;

public interface INotificationCommandDto
{
    public DateOnly ReceivedDate { get; }
    public DateOnly? DueDate { get; }
    public DateOnly? SentDate { get; }
    public NotificationType NotificationType { get; }
    public bool FollowupTaken { get; }
}
