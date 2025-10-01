using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class Notification : WorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Notification() { }

    internal Notification(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        WorkEntryType = WorkEntryType.Notification;
        Close(user);
    }

    // Properties

    public NotificationType NotificationType { get; set; } = null!;

    private DateOnly _receivedDate;

    public DateOnly ReceivedDate
    {
        get => _receivedDate;
        set
        {
            _receivedDate = value;
            EventDate = value;
        }
    }

    public DateOnly? DueDate { get; set; }
    public DateOnly? SentDate { get; set; }
    public bool FollowupTaken { get; set; }
}
