using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public record ActionTypeDto
{
    public EnforcementActionType ActionType { get; init; }
}
