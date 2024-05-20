using AirWeb.Domain.Entities.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Fces;

public record FceCreateDto
{
    [Required]
    [Display(Name = "Facility")]
    public FacilityId? FacilityId { get; init; }

    [Required]
    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Required]
    [Display(Name = "Reviewed by")]
    public string? ReviewedById { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
    [Display(Name = "Date Completed")]
    public DateOnly CompletedDate { get; init; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [Display(Name = "With on-site inspection")]
    public bool? OnsiteInspection { get; init; }
        
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string Notes { get; init; } = string.Empty;
}
