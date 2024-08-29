using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceUpdateDto
{
    [Required]
    [Display(Name = "Reviewed by")]
    public string? ReviewedById { get; init; }

    [Required]
    [Display(Name = "With on-site inspection")]
    public bool OnsiteInspection { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public required string Notes { get; init; }
}
