using AirWeb.Domain.Identity;
using AirWeb.Domain.Lookups.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public class Notification : ComplianceWork
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Notification() { }

    internal Notification(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        ComplianceWorkType = ComplianceWorkType.Notification;
        Close(user);
    }

    // Properties

    public NotificationType NotificationType { get; set; } = null!;

    public DateOnly ReceivedDate
    {
        get;
        set
        {
            field = value;
            EventDate = value;
        }
    }

    public DateOnly? DueDate { get; set; }
    public DateOnly? SentDate { get; set; }
    public bool FollowupTaken { get; set; }
}
