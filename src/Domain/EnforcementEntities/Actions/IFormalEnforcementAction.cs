using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.Domain.EnforcementEntities.Actions;

// Formal Enforcement Actions include:
// * Consent Orders
// * Administrative Orders

public interface IFormalEnforcementAction
{
    public CaseFile CaseFile { get; }
    public ICollection<IInformalEnforcementAction> ActionsAddressed { get; }
    public bool IsExecuted { get; }
    public DateOnly? ExecutedDate { get; }
    public bool IsResolved { get; }
    public DateOnly? ResolvedDate { get; }
    public OrderResolvedLetter? ResolvedLetter { get; }
}
