using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionCommand;

public record CreateEnforcementActionDto
{
    [DataType(DataType.MultilineText)]
    [StringLength(7000)]
    public string? Comment { get; init; }

    [Display(Name = "Response requested")]
    public bool ResponseRequested { get; init; } = true;

    public EnforcementActionType ActionType { get; init; }
}
