using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;

public record LonViewDto : ResponseRequestedViewDto, IIsResolved
{
    public DateOnly? ResolvedDate { get; init; }
    public bool IsResolved => ResolvedDate.HasValue;
}
