namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public interface ISourceTestReviewCommandDto
{
    public int ReferenceNumber { get; }
    public DateOnly ReceivedByCompliance { get; }
    public DateOnly? DueDate { get; }
    public bool FollowupTaken { get; }
}
