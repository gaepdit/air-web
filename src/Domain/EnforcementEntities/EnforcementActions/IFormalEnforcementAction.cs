using AirWeb.Domain.DataExchange;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

// Formal Enforcement Actions include:
// * Consent Orders
// * Administrative Orders

public interface IFormalEnforcementAction : IResolvable, IIsExecuted, IDataExchange
{
    internal void Execute(DateOnly executedDate);
}

public interface IIsExecuted
{
    public DateOnly? ExecutedDate { get; }
    public bool IsExecuted { get; }
}

public interface IIsAppealed
{
    public DateOnly? AppealedDate { get; }
    public bool IsAppealed { get; }
}
