namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

// Formal Enforcement Actions include:
// * Consent Orders
// * Administrative Orders

public interface IFormalEnforcementAction : IResolvable, IIsExecuted
{
    internal void Execute(DateOnly executedDate);
}

public interface IIsExecuted
{
    public DateOnly? ExecutedDate { get; }
    public bool IsExecuted { get; }
}
