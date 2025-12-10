using AirWeb.AppServices.CommonDtos;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionCommand;

public record EnforcementActionCreateDto : CommentDto
{
    [Display(Name = "Response Requested")]
    public bool ResponseRequested { get; init; } = true;

    public EnforcementActionType ActionType { get; init; }
    public bool IsReportableAction => EnforcementAction.ActionTypeIsReportable(ActionType);
}
