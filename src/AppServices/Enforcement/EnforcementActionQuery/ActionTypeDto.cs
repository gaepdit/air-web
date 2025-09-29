using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record ActionTypeDto
{
    public EnforcementActionType ActionType { get; init; }
}
