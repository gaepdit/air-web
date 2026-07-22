using AirWeb.AppServices.Core.Utilities;
using GaEpd.AppLibrary.DataAttributes;

namespace AirWeb.AppServices.Compliance.Enforcement.CaseFileCommand;

public record CaseFileCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }

    public int? EventId { get; init; }

    [Required]
    [Display(Name = "Staff Responsible")]
    public string? ResponsibleStaffId { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = DateTimeFormats.DateOnlyInput, ApplyFormatInEditMode = true)]
    [Display(Name = "Discovery Date")]
    [MaxDate]
    public DateOnly DiscoveryDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Case File Notes")]
    public string? CaseFileNotes { get; init; }

    [Display(Name = "Initial Enforcement Action (optional)")]
    public string? ActionType { get; init; }

    [Display(Name = "Response Requested")]
    public bool ResponseRequested { get; init; } = true;

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Enforcement Action Notes")]
    public string? EnforcementActionNotes { get; init; }
}
