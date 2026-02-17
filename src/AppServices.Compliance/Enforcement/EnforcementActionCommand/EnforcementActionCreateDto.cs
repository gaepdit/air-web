using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;

public record EnforcementActionCreateDto : CommentDto
{
    [Display(Name = "Response Requested")]
    public bool ResponseRequested { get; init; } = true;

    public EnforcementActionType ActionType { get; init; }
    public bool IsReportableAction => EnforcementAction.ActionTypeIsReportable(ActionType);
}
