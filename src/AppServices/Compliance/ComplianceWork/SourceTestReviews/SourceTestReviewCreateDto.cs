using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.ComplianceWork.SourceTestReviews;

public record SourceTestReviewCreateDto : SourceTestReviewCommandDto, IWorkEntryCreateDto
{
    public bool TestReportIsClosed { get; set; }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
