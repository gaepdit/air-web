using AirWeb.AppServices.Utilities;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;

public record WorkEntryCommandDto : IWorkEntryCommandDto
{
    // Data
    [Required]
    [Display(Name = "Staff responsible")]
    public string? ResponsibleStaffId { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Date acknowledgment letter sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Notes { get; init; }
}
