using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;

public record SourceTestReviewCreateDto : SourceTestReviewCommandDto, IComplianceWorkCreateDto
{
    public bool TestReportIsClosed { get; set; }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
