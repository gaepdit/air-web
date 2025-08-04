namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public abstract record BaseSummaryDto
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Comments")]
    public string Notes { get; init; } = null!;
}
