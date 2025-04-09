using AirWeb.AppServices.CommonDtos;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionCommand;

public record EnforcementActionCreateDto : CommentDto
{
    [Display(Name = "Response requested")]
    public bool ResponseRequested { get; init; } = true;

    public EnforcementActionType ActionType { get; init; }
    public bool WouldBeReportable => EnforcementAction.ActionTypeIsReportable(ActionType);
}
