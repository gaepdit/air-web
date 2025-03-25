namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

// Formal Enforcement Actions include:
// * Consent Orders
// * Administrative Orders

public interface IFormalEnforcementAction : IResolvable
{
    public bool IsExecuted { get; }
    public DateOnly? ExecutedDate { get; }
}
