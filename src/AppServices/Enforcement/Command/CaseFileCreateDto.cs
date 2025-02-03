namespace AirWeb.AppServices.Enforcement.Command;

public record CaseFileCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }

    public int? EventId { get; init; }

    [Required]
    [Display(Name = "Staff responsible")]
    public string? ResponsibleStaffId { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Discovery date")]
    public DateOnly DiscoveryDate { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; }
}
