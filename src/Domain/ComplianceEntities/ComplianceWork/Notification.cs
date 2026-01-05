using AirWeb.Domain.Identity;
using AirWeb.Domain.Lookups.NotificationTypes;

namespace AirWeb.Domain.ComplianceEntities.ComplianceWork;

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
