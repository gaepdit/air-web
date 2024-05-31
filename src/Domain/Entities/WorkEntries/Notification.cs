using AirWeb.Domain.Entities.NotificationTypes;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Notification : BaseWorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Notification() { }

    internal Notification(int? id, NotificationType notificationType) : base(id)
    {
        WorkEntryType = WorkEntryType.Notification;
        NotificationType = notificationType;
    }

    public NotificationType NotificationType { get; set; } = default!;
    public DateOnly ReceivedDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public DateOnly? SentDate { get; set; }
    public bool FollowupTaken { get; set; }
}
