namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IResolvable : IIsResolved
{
    internal void Resolve(DateOnly resolvedDate);
}
