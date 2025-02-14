namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record AoViewDto : ActionViewDto
{
    [Display(Name = "Executed")]
    public DateOnly? ExecutedDate { get; init; }

    [Display(Name = "Appealed")]
    public DateOnly? AppealedDate { get; init; }

    [Display(Name = "Resolved")]
    public DateOnly? ResolvedDate { get; init; }
}
