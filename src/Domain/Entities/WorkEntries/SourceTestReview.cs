namespace AirWeb.Domain.Entities.WorkEntries;

public class SourceTestReview : BaseComplianceEvent
{
    internal SourceTestReview(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.SourceTestReview;
    }

    public int ReferenceNumber { get; init; }
    public DateOnly ReceivedByCompliance { get; init; }
    public DateOnly? DueDate { get; init; }
    public bool FollowupTaken { get; init; }
}
