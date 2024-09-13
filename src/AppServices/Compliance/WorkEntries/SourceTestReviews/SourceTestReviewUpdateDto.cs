namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public record SourceTestReviewUpdateDto : SourceTestReviewCommandDto
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
