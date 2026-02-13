using AirWeb.AppServices.Core.Utilities;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;

public record ComplianceWorkCommandDto : IComplianceWorkCommandDto
{
    // Data
    [Required]
    [Display(Name = "Staff Responsible")]
    public string? ResponsibleStaffId { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date Acknowledgment Letter Sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Notes { get; init; }
}
