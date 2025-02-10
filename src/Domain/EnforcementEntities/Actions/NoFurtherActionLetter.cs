using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class NoFurtherActionLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoFurtherActionLetter() { }

    internal NoFurtherActionLetter(Guid id, IInformalEnforcementAction informalEnforcementAction, ApplicationUser? user)
        : base(id, informalEnforcementAction.CaseFile, user)
    {
        ActionType = EnforcementActionType.NoFurtherAction;
        ActionsAddressed.Add(informalEnforcementAction);
    }

    public ICollection<IInformalEnforcementAction> ActionsAddressed { get; init; } = [];
}
