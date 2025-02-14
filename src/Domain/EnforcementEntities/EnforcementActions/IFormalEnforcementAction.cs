namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

// Formal Enforcement Actions include:
// * Consent Orders
// * Administrative Orders

public interface IFormalEnforcementAction
{
    public bool IsExecuted { get; }
    public DateOnly? ExecutedDate { get; }
    public bool IsResolved { get; }
    public DateOnly? ResolvedDate { get; }
}
