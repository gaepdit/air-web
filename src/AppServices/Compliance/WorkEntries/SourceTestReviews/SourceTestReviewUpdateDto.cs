using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public record SourceTestReviewUpdateDto : WorkEntryUpdateDto, ISourceTestReviewCommandDto
{
    public bool IsClosed { get; init; }

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date Received")]
    public DateOnly ReceivedByCompliance { get; init; }

    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
