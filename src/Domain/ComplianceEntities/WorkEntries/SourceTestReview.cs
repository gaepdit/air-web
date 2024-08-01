namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public class SourceTestReview : ComplianceEvent
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private SourceTestReview() { }

    internal SourceTestReview(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.SourceTestReview;
        IsClosed = true;
    }

    // Properties

    public int ReferenceNumber { get; set; }
    public DateOnly ReceivedByCompliance { get; set; }
    public DateOnly? DueDate { get; set; }
    public bool FollowupTaken { get; set; }
}
