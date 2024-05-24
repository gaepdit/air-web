namespace AirWeb.Domain.Entities.WorkEntries;

public class SourceTestReview : BaseWorkEntry
{
    internal SourceTestReview(int? id) : base(id) => WorkEntryType = WorkEntryType.SourceTestReview;

    public int ReferenceNumber { get; init; }
    public DateOnly ReceivedByCompliance { get; init; }
    public DateOnly? DueDate { get; init; }
    public bool FollowupTaken { get; init; }
}
