using AirWeb.AppServices.CommonInterfaces;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public record SourceTestReviewUpdateDto : SourceTestReviewCommandDto, IIsClosedAndIsDeleted
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
