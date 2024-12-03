using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class Notification : WorkEntry
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Notification() { }

    internal Notification(int? id, ApplicationUser? user, FacilityId facilityId) : base(id, facilityId)
    {
        WorkEntryType = WorkEntryType.Notification;
        Close(user);
    }

    // Properties

    public NotificationType NotificationType { get; set; } = default!;
    public DateOnly ReceivedDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public DateOnly? SentDate { get; set; }
    public bool FollowupTaken { get; set; }
}
