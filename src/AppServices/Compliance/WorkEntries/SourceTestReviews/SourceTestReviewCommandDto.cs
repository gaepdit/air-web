using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Utilities;

namespace AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;

public abstract record SourceTestReviewCommandDto : WorkEntryCommandDto, ISourceTestReviewCommandDto
{
    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received By Compliance")]
    public DateOnly? ReceivedByComplianceDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
