using IaipDataService.Facilities;
using System.ComponentModel.DataAnnotations;

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
    [Display(Name = "Reviewed by")]
    public string? ReviewedById { get; init; }

    [Required]
    [Display(Name = "Included an on-site inspection")]
    public bool OnsiteInspection { get; init; } = true;

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; } = string.Empty;
}
