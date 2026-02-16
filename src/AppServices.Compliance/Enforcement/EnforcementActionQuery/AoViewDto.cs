using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public record AoViewDto : ReportableActionViewDto, IIsResolved, IIsExecuted, IIsAppealed
{
    [Display(Name = "Executed")]
    public DateOnly? ExecutedDate { get; init; }

    public bool IsExecuted => ExecutedDate.HasValue;

    [Display(Name = "Appealed")]
    public DateOnly? AppealedDate { get; init; }

    public bool IsAppealed => AppealedDate.HasValue;

    [Display(Name = "Resolved")]
    public DateOnly? ResolvedDate { get; init; }

    public bool IsResolved => ResolvedDate.HasValue;
}
