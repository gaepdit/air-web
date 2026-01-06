using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;

public record SourceTestReviewViewDto : ComplianceEventViewDto
{
    [Display(Name = "Reference Number")]
    public int? ReferenceNumber { get; init; }

    [Display(Name = "Date Received")]
    public DateOnly ReceivedByComplianceDate { get; init; }

    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-Up Action Taken")]
    public bool FollowupTaken { get; init; }
}
