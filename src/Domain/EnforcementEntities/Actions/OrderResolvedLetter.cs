using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class OrderResolvedLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private OrderResolvedLetter() { }

    internal OrderResolvedLetter(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.OrderResolvedLetter;
    }

    internal OrderResolvedLetter(Guid id, IFormalEnforcementAction formalEnforcementAction, ApplicationUser? user)
        : base(id, formalEnforcementAction.CaseFile, user)
    {
        ActionType = EnforcementActionType.OrderResolvedLetter;
        FormalEnforcementActions.Add(formalEnforcementAction);
    }

    public ICollection<IFormalEnforcementAction> FormalEnforcementActions { get; init; } = [];
}
