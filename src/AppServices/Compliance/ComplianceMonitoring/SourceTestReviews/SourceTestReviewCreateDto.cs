using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;

public record SourceTestReviewCreateDto : SourceTestReviewCommandDto, IWorkEntryCreateDto
{
    public bool TestReportIsClosed { get; set; }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
