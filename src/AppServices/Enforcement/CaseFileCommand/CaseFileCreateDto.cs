using AirWeb.AppServices.Core.Utilities;

namespace AirWeb.AppServices.Enforcement.CaseFileCommand;

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
    public DateOnly DiscoveryDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; }
}
