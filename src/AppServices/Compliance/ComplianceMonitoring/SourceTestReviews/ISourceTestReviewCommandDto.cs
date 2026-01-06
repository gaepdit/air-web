namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;

public interface ISourceTestReviewCommandDto
{
    public DateOnly? ReceivedByComplianceDate { get; }
    public DateOnly? DueDate { get; }
    public bool FollowupTaken { get; }
}
