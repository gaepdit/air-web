namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;

public record WorkEntryCommandDto : IWorkEntryCommandDto
{
    // Data
    [Required]
    [Display(Name = "Staff responsible")]
    public string? ResponsibleStaffId { get; init; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date acknowledgment letter sent")]
    public DateOnly? AcknowledgmentLetterDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Notes { get; init; }
}
