using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceCreateDto
{
    public FceCreateDto() { }

    public FceCreateDto(FacilityId facilityId, string userId)
    {
        FacilityId = facilityId;
        ReviewedById = userId;
    }

    [Required]
    [Display(Name = "Facility")]
    public string? FacilityId { get; init; }

    [Required]
    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Required]
    [Display(Name = "Reviewed By")]
    public string? ReviewedById { get; init; }

    [Required]
    [Display(Name = "With On-Site Inspection")]
    public bool OnsiteInspection { get; init; } = true;

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; }
}
