using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class SourceTestReview : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private SourceTestReview() { }

    internal SourceTestReview(int? id, FacilityId facilityId, ApplicationUser? user = null)
        : base(id, facilityId, user)
    {
        WorkEntryType = WorkEntryType.SourceTestReview;
        Close(user);
    }

    // Properties

    public int ReferenceNumber { get; set; }

    private DateOnly _receivedByComplianceDate;

    public DateOnly ReceivedByComplianceDate
    {
        get => _receivedByComplianceDate;
        set
        {
            _receivedByComplianceDate = value;
            EventDate = value;
        }
    }

    public DateOnly? DueDate { get; set; }
    public bool FollowupTaken { get; set; }
}
