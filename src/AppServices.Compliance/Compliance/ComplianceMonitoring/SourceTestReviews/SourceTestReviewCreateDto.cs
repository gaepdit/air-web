using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.SourceTestReviews;

public record SourceTestReviewCreateDto : SourceTestReviewCommandDto, IComplianceWorkCreateDto
{
    // Validation fields
    public bool TestReportIsClosed { get; set; }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; set; }

    public int? CaseFileId => null;
}
