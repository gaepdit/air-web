using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public record SourceTestReviewCreateDto : SourceTestReviewCommandDto, IWorkEntryCreateDto
{
    public bool TestReportIsClosed { get; set; }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
