namespace AirWeb.AppServices.Compliance.Fces;

public record FceUpdateDto
{
    public FceUpdateDto() { }

    public FceUpdateDto(FceSummaryDto fce)
    {
        ReviewedById = fce.ReviewedBy?.Id;
        OnsiteInspection = fce.OnsiteInspection;
        Notes = fce.Notes;
    }

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
