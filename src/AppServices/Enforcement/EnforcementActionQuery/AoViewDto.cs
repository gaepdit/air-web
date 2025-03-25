using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record AoViewDto : ActionViewDto, IIsResolved
{
    [Display(Name = "Executed")]
    public DateOnly? ExecutedDate { get; init; }

    [Display(Name = "Appealed")]
    public DateOnly? AppealedDate { get; init; }

    [Display(Name = "Resolved")]
    public DateOnly? ResolvedDate { get; init; }

    public bool IsResolved => ResolvedDate.HasValue;
}
