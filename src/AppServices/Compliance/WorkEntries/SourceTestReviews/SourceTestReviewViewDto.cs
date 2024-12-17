using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public record SourceTestReviewViewDto : WorkEntryViewDto
{
    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date Received")]
    public DateOnly ReceivedByComplianceDate { get; init; }

    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
