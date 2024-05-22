using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.WorkEntries.SourceTestReviews;

public record SourceTestReviewCreateDto : BaseWorkEntryCreateDto
{
    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received")]
    public DateOnly ReceivedByCompliance { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
