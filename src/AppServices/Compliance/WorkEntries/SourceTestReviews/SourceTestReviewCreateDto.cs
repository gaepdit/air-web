using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public record SourceTestReviewCreateDto : SourceTestReviewCommandDto, IWorkEntryCreateDto
{
    public bool TestReportIsClosed { get; set; }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }
}
