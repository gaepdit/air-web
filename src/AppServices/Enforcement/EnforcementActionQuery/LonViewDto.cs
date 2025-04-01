using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement.EnforcementActionQuery;

public record LonViewDto : ResponseRequestedViewDto, IIsResolved
{
    public DateOnly? ResolvedDate { get; init; }
    public bool IsResolved => ResolvedDate.HasValue;
}
