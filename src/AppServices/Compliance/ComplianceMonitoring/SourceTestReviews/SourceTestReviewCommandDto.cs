using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Core.Utilities;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;

public abstract record SourceTestReviewCommandDto : ComplianceWorkCommandDto, ISourceTestReviewCommandDto
{
    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Received By Compliance")]
    public DateOnly? ReceivedByComplianceDate { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Test Due Date")]
    public DateOnly? DueDate { get; init; }

    [Display(Name = "Follow-up Action Taken")]
    public bool FollowupTaken { get; init; }
}
