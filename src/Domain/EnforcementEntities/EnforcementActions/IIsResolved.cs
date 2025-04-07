namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public interface IIsResolved
{
    public DateOnly? ResolvedDate { get; }
    public bool IsResolved { get; }
}

public interface IResolvable : IIsResolved
{
    internal void Resolve(DateOnly resolvedDate);
}
