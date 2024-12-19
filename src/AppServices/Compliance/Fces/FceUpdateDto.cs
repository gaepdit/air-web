using AirWeb.AppServices.CommonInterfaces;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceUpdateDto : IIsDeleted
{
    // Authorization handler assist properties
    public bool IsDeleted { get; init; }

    // Data properties
    [Required]
    [Display(Name = "Reviewed by")]
    public string? ReviewedById { get; init; }

    [Required]
    [Display(Name = "With on-site inspection")]
    public bool OnsiteInspection { get; init; }

    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    [Display(Name = "Notes")]
    public string? Notes { get; init; }
}
