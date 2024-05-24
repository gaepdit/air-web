namespace AirWeb.Domain.Entities.WorkEntries;

public class SourceTestReview : BaseComplianceEvent
{
    internal SourceTestReview(int? id) : base(id)
    {
        WorkEntryType = WorkEntryType.ComplianceEvent;
        ComplianceEventType = ComplianceEventType.SourceTestReview;
    }

    public int ReferenceNumber { get; set; }
    public DateOnly ReceivedByCompliance { get; set; }
    public DateOnly? DueDate { get; set; }
    public bool FollowupTaken { get; set; }
}
