using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.SourceMonitoringReviews;

public record SourceMonitoringReviewUpdateDto : BaseWorkEntryUpdateDto
{
    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date Received")]
    public DateOnly ReceivedByCompliance { get; init; }

    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
